
reset

$40010000 constant AFIO
AFIO $0 + constant AFIO_EVCR
AFIO $4 + constant AFIO_MAPR
AFIO $8 + constant AFIO_EXTICR1
AFIO $C + constant AFIO_EXTICR2
AFIO $10 + constant AFIO_EXTICR3
AFIO $14 + constant AFIO_EXTICR4
AFIO $1C + constant AFIO_MAPR2

$40010400 constant EXTI
EXTI $0 + constant EXTI_IMR \ Interrupt mask register
EXTI $4 + constant EXTI_EMR \ Event mask register
EXTI $8 + constant EXTI_RTSR \ Rising trigger selection register
EXTI $C + constant EXTI_FTSR \ Falling trigger selection register
EXTI $10 + constant EXTI_SWIER \ Software interrupt event register
EXTI $14 + constant EXTI_PR \ Pending register

%0000 constant PA_CR
%0001 constant PB_CR
%0010 constant PC_CR
%0011 constant PD_CR
%0100 constant PE_CR
%0101 constant PF_CR
%0110 constant PG_CR


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

: exti.set ( t/f pin Px_CR -- )
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




: irq.exti.4.handler
  ." ."
  \ Clear Bit pending
;

: init.gpios
  LED.RED 1 gpio.set
  LED.GREEN 1 gpio.set
  LED.BLUE 1 gpio.set
  IR.IC 0 gpio.set
  ' irq.exti.4.handler irq-exti4 @ \ Set IR interrupt handler.
;


0 variable test-v

: test test-v ! ;

' test USER-WORD !

: stk.current ( -- 24bit-systick-value ) \ The current systick value
  STK_CVR @
;

: stk.reload.set ( 24bit-val -- ) \ Set systick reload value
  STK_RVR !
;


\ ------------- testing code ----------

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

: prc ( ms_counter -- )
  dup
  dup 1000 mod 0= if led.blue.on drop else drop then
  dup 1000 mod 250 = if led.blue.off drop else drop then
;

' systick-handler irq-systick !    \ This 'hooks' the systick-handler word (above) to the systick irq

' prc USER-WORD !

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

: t.ir
  init.gpios
  INIT-SYSTICK \ Initialize timer and enable its interrupt


  \ GPIOA GPIO.CRL CRb.
  ." Shiffing till a key pressed." cr
  timer.current@ frame.current !
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
