
reset

$40010000 constant AFIO
AFIO $0 + constant AFIO_EVCR
AFIO $4 + constant AFIO_MAPR
AFIO $8 + constant AFIO_EXTICR1
AFIO $C + constant AFIO_EXTICR2
AFIO $10 + constant AFIO_EXTICR3
AFIO $14 + constant AFIO_EXTICR4
AFIO $1C + constant AFIO_MAPR2

$40021000 constant RCC
RCC $0 + constant RCC_CR
RCC $4 + constant RCC_CFGR
RCC $8 + constant RCC_CIR
RCC $C + constant RCC_APB2RSTR
RCC $10 + constant RCC_APB1RSTR
RCC $14 + constant RCC_AHBENR
RCC $18 + constant RCC_APB2ENR \ Peripheral clock enabler for GPIOx.
RCC $1C + constant RCC_APB1ENR
RCC $20 + constant RCC_BDCR
RCC $24 + constant RCC_CSR


$40010400 constant EXTI
EXTI $0 + constant EXTI_IMR \ Interrupt mask register
EXTI $4 + constant EXTI_EMR \ Event mask register
EXTI $8 + constant EXTI_RTSR \ Rising trigger selection register
EXTI $C + constant EXTI_FTSR \ Falling trigger selection register
EXTI $10 + constant EXTI_SWIER \ Software interrupt event register
EXTI $14 + constant EXTI_PR \ Pending register

$E000E000 constant NVIC
NVIC $4 + constant NVIC_ICTR
NVIC $F00 + constant NVIC_STIR
NVIC $100 + constant NVIC_ISER0
NVIC $104 + constant NVIC_ISER1
NVIC $180 + constant NVIC_ICER0
NVIC $184 + constant NVIC_ICER1
NVIC $200 + constant NVIC_ISPR0
NVIC $204 + constant NVIC_ISPR1
NVIC $280 + constant NVIC_ICPR0
NVIC $284 + constant NVIC_ICPR1
NVIC $300 + constant NVIC_IABR0
NVIC $304 + constant NVIC_IABR1
NVIC $400 + constant NVIC_IPR0
NVIC $404 + constant NVIC_IPR1
NVIC $408 + constant NVIC_IPR2
NVIC $40C + constant NVIC_IPR3
NVIC $410 + constant NVIC_IPR4
NVIC $414 + constant NVIC_IPR5
NVIC $418 + constant NVIC_IPR6
NVIC $41C + constant NVIC_IPR7
NVIC $420 + constant NVIC_IPR8
NVIC $424 + constant NVIC_IPR9
NVIC $428 + constant NVIC_IPR10
NVIC $42C + constant NVIC_IPR11
NVIC $430 + constant NVIC_IPR12
NVIC $434 + constant NVIC_IPR13
NVIC $438 + constant NVIC_IPR14


%0000 constant PA_CR
%0001 constant PB_CR
%0010 constant PC_CR
%0011 constant PD_CR
%0100 constant PE_CR
%0101 constant PF_CR
%0110 constant PG_CR

: exti.enable ( Px_CR -- )
  1 swap 2 + lshift
  RCC_APB2ENR bis!
;

: exti.reg.conf ( pin -- lshift AFIO_EXTCRx )
  dup
  2 rshift swap \ pin>>2 pin
  $3 and 2 lshift \ pin>>2 pin*4
  swap     \ lhift pin>>2
  dup
  0=
  if
    drop
    AFIO_EXTICR1
    exit
  then
  dup
  1 =
  if
    drop
    AFIO_EXTICR2
    exit
  then
  dup
  2 =
  if
    drop
    AFIO_EXTICR3
    exit
  then
  drop
  AFIO_EXTICR4
;

: exti.conf ( t/f pin Px_CR -- )
  swap \ t/f Px p
  exti.reg.conf \ t/f Px lshift AFIO_EXTICRx
  >r
  dup $F swap lshift
  r@ bic!
  rot \ Px lshift t/f
  if
    lshift
    r> bis!
  else
    2drop
    rdrop
  then
;

: exti.pr.pending? ( no -- t/f ) \ whether the interrupt is pending for INT no
  EXTI_PR @ swap rshift 1 and
  0<>
;

: exti.pr.clear ( no -- ) \ Clears pending bit for INT no
  1 swap lshift
  EXTI_PR bis!
;

: exti.set ( t/f no EXTI_x -- )
  >r
  1 swap lshift
  r>     \ t/f 1<<no EXTI_x
  rot
  if
    bis! \ Set bit
  else
    bic! \ Clear bit
  then
;

: exti.mask ( t/f no EXTI_x -- )
  rot not -rot
  exti.set
;

: exti.imr.mask ( t/f no -- )
  EXTI_IMR
  exti.mask
;

: exti.emr.mask ( t/f no -- )
  EXTI_EMR
  exti.mask
;

: exti.rtsr.trigger ( t/f no -- )
  EXTI_RTSR
  exti.set
;

: exti.ftsr.trigger ( t/f no -- )
  EXTI_FTSR
  exti.set
;

: exti.event.trigger ( no -- )
  true swap
  EXTI_SWIER
  exti.set
;

: IR.IC.PIN 4 ; \ TODO: Remove after full debug

\ --- timer ----

: stk.current ( -- 24bit-systick-value ) \ The current systick value
  STK_CVR @
;

: stk.reload.set ( 24bit-val -- ) \ Set systick reload value
  STK_RVR !
;


\ ----------------------------- LOCAL PART ----------------------------

0 variable frame.current
: timer.current@ ms_counter @ ;
0 variable stk.prev
false variable irframe

: ir.endframe
    timer.current@
    frame.current @
    - 2 >
    if
      irframe @
      if
        ." --->" cr
        false irframe !
      then
    then
;

: timing
  timer.current@
  frame.current @
  - .
  stk.current stk.prev @ -
  dup 0< if 8080 + then
  .

  stk.current stk.prev !

  timer.current@
  frame.current !
;

true variable IR.IC.PREV

: irq.exti.4.handler
  \ Clear Bit pending
  IR.IC.PIN exti.pr.clear
  \ IR.IC gpio.in
  \ not LED.GREEN gpio.out
  IR.IC gpio.in
  dup not LED.GREEN gpio.out
  \ dup LED.BLUE gpio.out

  dup if
    true irframe ! \ Something changed -> ir frame
    \ timing

    \ if 43 else 45 then emit \ + -

  then

  ir.endframe

  IR.IC.PREV !
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
  dup
  dup 1000 mod 0= if led.blue.on drop else drop then
  dup 1000 mod 250 = if led.blue.off drop else drop then
;

' systick-handler irq-systick !    \ This 'hooks' the systick-handler word (above) to the systick irq

' prc USER-WORD !

: init ( enable-int -- ) \ Initialize IR processing
  init.gpios
  INIT-SYSTICK \ Initialize timer and enable its interrupt
  timer.current@ frame.current !
;

: t.i
  true init
;

: t.ir
  false init

  \ GPIOA GPIO.CRL CRb.
  ." Shiffing till a key pressed." cr
  -1 >r \ Previous value of ir
  begin
    IR.IC gpio.in
    not
    dup LED.GREEN gpio.out
    dup LED.RED gpio.out
    \ dup LED.BLUE gpio.out
    not

    dup r@ <>
    if
      rdrop dup >r
      true irframe ! \ Something changed -> ir frame
      timing


      if 43 else 45 then emit \ + -

    else
      drop
    then

    ir.endframe

    key? until
  rdrop
  \ led.green.off
  timer.current@ . cr
  \ DISABLE-SYSTICK
;
