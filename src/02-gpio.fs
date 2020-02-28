compiletoflash

$40010800 constant GPIOA
$40010C00 constant GPIOB
$40011000 constant GPIOC
\ $40011400 constant GPIOD
\ $40011800 constant GPIOE
\ $40011C00 constant GPIOF
\ $40012000 constant GPIOG

: GPIO.CRL ( addr -- addr ) ;
: GPIO.CRH ( n -- n ) $04 + ;
: GPIO.IDR ( n -- n ) $08 + ;
: GPIO.ODR ( n -- n ) $0C + ;
: GPIO.BSRR ( n -- n ) $10 + ;
: GPIO.BRR ( n -- n ) $14 + ;

: prep.cnf$ ( o -- config-code )
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
  prep.cnf$ \ add cnf configuration depending out 0|1
  swap
  lshift   \ 0co0
  r@ @ or  \ xcox
  r> !
;


: prep.cnf.af$ ( o -- config-code-alternate-function )
  dup 0= if
    %0100 or \ Input mode m=00!
  else
    %1000 or \ Output mode c=10 -> AF_PP m=00 out 20Mhz
  then
;

: gpio.af.set ( npin addr out? -- )
  -rot GPIO.CRL >r \ out? npin
  2 lshift \ o np*4
  dup      \ o np4 np4
  $f swap  \ o np4 $F np4
  lshift not  \ o np4 F0FFF
  r@ @ and \ o np4 F0F&av
  r@ !     \ o np4
  swap
  prep.cnf.af$ \ add cnf configuration depending out 0|1
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

compiletoram
