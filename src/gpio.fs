
$40010800 constant GPIOA
$40010C00 constant GPIOB

: GPIO.CRL ( addr -- addr ) ;
: GPIO.CRH ( n -- n ) $04 + ;
: GPIO.IDR ( n -- n ) $08 + ;
: GPIO.ODR ( n -- n ) $0C + ;
: GPIO.BSRR ( n -- n ) $10 + ;
: GPIO.BRR ( n -- n ) $14 + ;

: gpio.set ( addr npin out -- n ) 
  swap \ addr out npin
  2 lshift
  lshift  \ addr code
  swap \ code addr
  GPIO.CRL bis! 
;

: .reg ( word -- ) binary . decimal ;

: init.gpios
  GPIOA 6 1 gpio.set
  GPIOA 7 1 gpio.set
  GPIOB 0 1 gpio.set
;




