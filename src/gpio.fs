
reset


: init.gpios
  LED.RED 1 gpio.set
  LED.GREEN 1 gpio.set
  LED.BLUE 1 gpio.set
  IR.IC 0 gpio.set
;


\ ------------- testing code ----------

: sleep
  50000 0 do nop loop
;

: gcdsleep
  80 0 do loop
;

: TEST.PIN ( -- GPIOA pin ) GPIOA 4 ;

: t init.gpios ;

0 variable gcdtimer
0 variable gcdcurrent
false variable irframe

: ir.endframe
    gcdtimer @
    gcdcurrent @
    - 100 >
    if
      irframe @
      if
        cr
        false irframe !
      then
    then
;

: t.ir
  \ init.gpios
  t

  \ GPIOA GPIO.CRL CRb.
  ." Shiffing till a key pressed." cr
  0 gcdtimer !
  0 gcdcurrent !
  -1 >r \ Previous value of ir
  begin
    IR.IC gpio.in
    not
    dup LED.GREEN gpio.out
    dup LED.RED gpio.out
    dup LED.BLUE gpio.out
    not

    dup r@ <>
    if

      true irframe ! \ Something changed -> ir frame

      rdrop dup >r

      gcdtimer @
      gcdcurrent @
      - .

      if 43 else 45 then emit \ + -

      gcdtimer @
      gcdcurrent !

    else
      drop
    then

    gcdsleep
    gcdtimer @ 1+ gcdtimer !
    ir.endframe

    key? until
  rdrop
  led.green.off
  gcdcurrent @ . cr
;
