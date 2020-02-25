compiletoflash

$40010800 constant GPIOA
$40010C00 constant GPIOB

: GPIO.CRL ( addr -- addr ) ;
: GPIO.CRH ( n -- n ) $04 + ;
: GPIO.IDR ( n -- n ) $08 + ;
: GPIO.ODR ( n -- n ) $0C + ;
: GPIO.BSRR ( n -- n ) $10 + ;
: GPIO.BRR ( n -- n ) $14 + ;

: prep.cnf
  dup 0= if
    %0100 or
  then
;

: gpio.set ( npin addr out? -- )
  -rot GPIO.CRL >r \ out? npin
  2 lshift \ o np*4
  dup      \ o np4 np4
  $f swap  \ o np4 $F np4
  lshift not  \ o np4 F0FFF
  r@ @ and \ o np4 F0F&av
  r@ !     \ o np4
  swap
  prep.cnf \ add cnf configuration depending out 0|1
  swap
  lshift   \ 0co0
  r@ @ or  \ xcox
  r> !
;

: gpio.out ( out? pin GPIOX -- )
  GPIO.ODR >r     \ pin out
  dup 1           \ o p p 1
  swap lshift not \ o p 1110111
  r@ @      \ o p 11110111 xxxxx
  and       \ o p xxxx0xxx
  -rot      \ xxxx0xxx o p
  lshift or \ xxxxoxxxx
  r> !
;

: gpio.in.val ( pin GPIOx -- pin VAL )
  GPIO.IDR @
;

: gpio.in ( pin GPIOX -- 0|-1 )
  gpio.in.val \ pin xxxvxxx
  over \ pin xvx pin
  1 swap \ pin xvx 1 pin
  lshift and \ pin 000v000
  swap rshift \ 00000v
  0<>
;

: reg. ( word -- ) bin. ;

: LED.RED ( -- pin GPIOx ) 6 GPIOA ;
: LED.GREEN ( -- pin GPIOx ) 7 GPIOA ;
: LED.BLUE ( -- pin GPIOx ) 0 GPIOB ;
: IR.IC.PIN 4 ;
: IR.IC ( -- pin GPIOx ) IR.IC.PIN GPIOB ;

: led.on ( pin GPIOX -- ) 1 -rot gpio.out ;
: led.off ( pin GPIOX -- ) 0 -rot gpio.out ;

: led.red.on LED.RED led.on ;
: led.red.off LED.RED led.off ;
: led.green.on LED.GREEN led.on ;
: led.green.off LED.GREEN led.off ;
: led.blue.on LED.BLUE led.on ;
: led.blue.off LED.BLUE led.off ;

: led.sleep 150000 0 do nop loop ;  ok.
: blink.test ( GPIOX pin -- )
  begin
    2dup led.on
    led.sleep
    2dup led.off
    led.sleep
    key? until
  2drop
;

compiletoram
