
reset

\ ----------------------------- LOCAL PART ----------------------------

0 variable NEC.DECODE
true variable NEC.CODE
true variable NEC.STATE
48 constant NEC.LAST.STATE

: nec.state- ( -- )
  NEC.STATE @ 1- NEC.STATE !
;

: nec.code.add ( n -- )
  NEC.DECODE @ 1 lshift or
  NEC.DECODE !
;

: nec.code.1 ( -- )
  1 nec.code.add
;

: nec.code.0 ( -- )
  0 nec.code.add
;

: nec.dec.down ( 10ms -- )
  NEC.STATE @ true =
  if
    NEC.LAST.STATE NEC.STATE ! \ Start decoding
    0 NEC.DECODE ! \ Initial value
    drop   \ Did not use time delta (10ms)
    exit
  then
  nec.state-
  10 >=
  if
    \ ." 1"
    nec.code.1
  else
    \ ." 0"
    nec.code.0
  then
;

: nec.dec.up ( 10ms -- )
  nec.state-
  drop
;

: nec.dec ( 10ms up|down -- )
  \ up = true, down = false
  if \ up
    nec.dec.up
  else
    nec.dec.down
  then
;

: nec.accept
  NEC.DECODE @ NEC.CODE !
;

: nec.reset
  true NEC.STATE !
  0 NEC.DECODE !
  \ true NEC.CODE !
;

: nec.key
  NEC.CODE @ $FF and
  case
    $9d of $5e endof \ "^" Up
    $57 of $76 endof \ "v" Down
    $DD of $3c endof \ "<" Left
    $3d of $3e endof \ ">" Right
    $fd of $6b endof \ "k" Ok
    $97 of $31 endof \ "1"
    $67 of $32 endof \ "2"
    $4f of $33 endof \ "3"
    $cf of $34 endof \ "4"
    $e7 of $35 endof \ "5"
    $85 of $36 endof \ "6"
    $ef of $37 endof \ "7"
    $c7 of $38 endof \ "8"
    $a5 of $39 endof \ "9"
    $b5 of $30 endof \ "0"
    $bd of $2a endof \ "*"
    $ad of $23 endof \ "#"
  endcase
;


0 variable frame.current
: timer.current ms_counter ;
0 variable stk.prev
false variable irframe
0 variable USER-ACCEPT
true variable last.space

: ir.endframe ( -- )
  timer.current @
  frame.current @
  - dup 100 >
  if
    irframe @
    if
      last.space !
      false irframe !
      nec.accept
      nec.reset
      USER-ACCEPT @
      dup 0<> if execute else drop then
    else
      drop
    then
  else
    drop
  then
;

: timing ( -- dmS )
  timer.current @
  dup
  frame.current @
  -
  swap
  frame.current !
;

: ir.irq.disable
  true IR.IC.PIN exti.imr.mask      \ Mask the interrupt
;

: ir.irq.enable
  false IR.IC.PIN exti.imr.mask      \ Mask the interrupt
;

: irq.exti.4.handler
  \ Clear Bit pending
  ir.irq.disable

  IR.IC.PIN exti.pr.clear
  IR.IC gpio.in

  dup not LED.GREEN gpio.out

  dup if \ "/"
  else
    true irframe ! \ Something changed -> ir frame
  then

  timing \ \/ dmS
  swap

  nec.dec

  ir.irq.enable
;

false variable LED.ON.ON

: user-accept-prc ( -- )
  \ NEC.CODE crb.
  nec.key
  dup . dup emit
  case
    $6b of \ "Ok" aka "k"
      LED.ON.ON @
      if
        led.red.off
        false LED.ON.ON !
      else
        led.red.on
        true LED.ON.ON !
      then
    endof
  endcase
;

: init.PB4.isr \ Init interrupt processing for \ / PB4.
  ['] irq.exti.4.handler irq-exti4 ! \ Set IR interrupt handler.
  true IR.IC.PIN PB_CR exti.conf     \ Configure EXTI line
  false IR.IC.PIN exti.imr.mask      \ Unmask the interrupt
  false IR.IC.PIN exti.emr.mask      \ Unmask the event line FIXME: comment out
  true IR.IC.PIN exti.rtsr.trigger   \ Set the rising edge trigger
  true IR.IC.PIN exti.ftsr.trigger   \ Set the falling edge trigger
  PB_CR exti.enable                  \ Enable peripheral clock
  1 10 lshift NVIC_ISER0 bis!
  nec.reset \ reset NEC data
  ['] user-accept-prc USER-ACCEPT !
;

: init.gpios ( int-on-off -- ) \ Enable interrupt-driven procs ?
  LED.RED 1 gpio.set
  LED.GREEN 1 gpio.set
  LED.BLUE 1 gpio.set
  IR.IC 0 gpio.set
  if
    init.PB4.isr
  then
;


0 variable test-v

: test test-v ! ;

' test USER-WORD !


\ ------------- testing code ----------

: prc ( ms_counter -- )
  \ drop exit
  dup
  dup 10000 mod 0= if led.blue.on drop else drop then
  dup 10000 mod 2500 = if led.blue.off drop else drop then

  ir.endframe
;

' systick-handler irq-systick !    \ This 'hooks' the systick-handler word (above) to the systick irq

' prc USER-WORD !

: INIT-SYSTICK-1
  INIT-SYSTICK
  808 STK_RVR !			\ systick calib for 1ms using internal 8mhz osc
;


: init ( enable-int -- ) \ Initialize IR processing
  init.gpios
  INIT-SYSTICK-1 \ Initialize timer and enable its interrupt
  timer.current @ frame.current !
;

: t.i
  true init
;
