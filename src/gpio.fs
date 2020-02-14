\ Program Name: 1b.fs  for Mecrisp-Stellaris by Matthias Koch and licensed under the GPL
\ Copyright 2019 t.porter <terry@tjporter.com.au> and licensed under the BSD license.
\ This program must be loaded before memmap.fs as it provided the pretty printing legend for generic 32 bit prints
\ Also included is "bin." which prints the binary form of a number with no spaces between numbers for easy copy and pasting purposes

reset
compiletoram

\ compiletoflash

: b32loop. ( u -- ) \ print 32 bits in 1 bit groups
0  <#
31 0 DO
# 32 HOLD LOOP
# #>
TYPE ;

: b32sloop. ( u -- ) \ print 32 bits in 1 bit groups
 0  <#
 31 0 DO
 # LOOP
 # #>
 TYPE ;


: 1b. ( u -- ) cr \ Label 1 bit generic groups
      @ dup hex. cr
      ." 3|3|2|2|2|2|2|2|2|2|2|2|1|1|1|1|1|1|1|1|1|1|" cr
      ." 1|0|9|8|7|6|5|4|3|2|1|0|9|8|7|6|5|4|3|2|1|0|9|8|7|6|5|4|3|2|1|0 " cr
      binary b32loop. decimal cr
;

: CRb. ( u -- ) cr \ Label 1 bit generic groups
      @ dup hex. cr
      ." 3|3|2|2|2|2|2|2|2|2|2|2|1|1|1|1|1|1|1|1|1|1|0|0|0|0|0|0|0|0|0|0|" cr
      ." 1|0|9|8|7|6|5|4|3|2|1|0|9|8|7|6|5|4|3|2|1|0|9|8|7|6|5|4|3|2|1|0|" cr
      ." ---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+" cr
      ." c 7|m 7|c 6|m 6|c 5|m 5|c 4|m 4|c 3|m 3|c 2|m 2|c 1|m 1|c 0|m 0|" cr
      ." ---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+" cr
      binary b32loop. decimal cr
;

: bin. cr	  \ for manual operations producing easy to paste binary numbers i.e. " 10 bin. "
       dup hex. cr
       ." 3322222222221111111111" cr
       ." 10987654321098765432109876543210 " cr
       binary b32sloop. decimal cr
;

\ compiletoram



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
  dup
  $f swap
  lshift  \ addr code
  .s
  bin.
  \ swap \ code addr
  \ GPIO.CRL bis!
;

: reg. ( word -- ) bin. ;

: init.gpios
  GPIOA 6 1 gpio.set
  GPIOA 7 1 gpio.set
  GPIOB 0 1 gpio.set
;

: t init.gpios ;
