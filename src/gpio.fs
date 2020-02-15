reset

$40010800 constant GPIOA
$40010C00 constant GPIOB

: GPIO.CRL ( addr -- addr ) ;
: GPIO.CRH ( n -- n ) $04 + ;
: GPIO.IDR ( n -- n ) $08 + ;
: GPIO.ODR ( n -- n ) $0C + ;
: GPIO.BSRR ( n -- n ) $10 + ;
: GPIO.BRR ( n -- n ) $14 + ;

: gpio.set ( addr npin out? -- )
  rot GPIO.CRL >r
  swap     \ out? npin

  2 lshift \ o np*4
  dup      \ o np4 np4
  $f swap  \ o np4 $F np4
  lshift not  \ o np4 F0FFF
  r@ @ and \ o np4 F0F&av
  r@ !     \ o np4
  lshift   \ 0o0
  r@ @ or  \ xox
  r> !
;

: gpio.out ( GPIO-addr pin out? -- )
  rot GPIO.ODR >r \ pin out
  swap            \ out pin
  dup 1           \ o p p 1
  swap lshift not \ o p 1110111
  r@ @      \ o p 11110111 xxxxx
  and       \ o p xxxx0xxx
  -rot      \ xxxx0xxx o p
  lshift or \ xxxxoxxxx
  r> !
;

: reg. ( word -- ) bin. ;

: init.gpios
  GPIOA 6 1 gpio.set
  GPIOA 7 1 gpio.set
  GPIOB 0 1 gpio.set
;

: LED.RED ( -- GPIOA pin ) GPIOA 6 ;
: LED.GREEN ( -- GPIOA pin ) GPIOA 7 ;
: LED.BLUE ( -- GPIOB pin ) GPIOB 0 ;

: led.on ( GPIOX pin -- ) 1 gpio.out ;
: led.off ( GPIOX pin -- ) 0 gpio.out ;

: led.red.on LED.RED led.on ;
: led.red.off LED.RED led.off ;
: led.green.on LED.GREEN led.on ;
: led.green.off LED.GREEN led.off ;
: led.blue.on LED.BLUE led.on ;
: led.blue.off LED.BLUE led.off ;

: led.sleep 150000 0 do nop loop ;  ok.
: blink.test ( GPIOX pin -- ) begin 2dup led.on led.sleep 2dup led.off led.sleep key? until 2drop ;

: t init.gpios ;
