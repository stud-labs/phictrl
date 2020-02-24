
reset


: init.gpios
  LED.RED 1 gpio.set
  LED.GREEN 1 gpio.set
  LED.BLUE 1 gpio.set
  IR.IC 0 gpio.set
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
