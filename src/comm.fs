\ COMMUNICATION USART1,2,3

reset

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

$40013800 constant USART1
$40004400 constant USART2 \ FIXME: Check DOC
$40004800 constant USART3 \ FIXME: Check DOC

\ TODO: REMOVE After completion
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


: USART.SR ( USARTx -- USARTx_SR ) ;  \ Status Register BUG FIX
: USART.DR ( USARTx -- USARTx_DR ) $04 + ; \ Data Register
: USART.BRR ( USARTx -- USARTx_BRR ) $08 + ; \ Bit Rate Register
: USART.CR1 ( USARTx -- USARTx_CR1 ) $0C + ; \ Control Register 1
: USART.CR2 ( USARTx -- USARTx_CR2 ) $10 + ; \ Control Register 2
: USART.CR3 ( USARTx -- USARTx_CR3 ) $14 + ; \ Control Register 3
: USART.GTPR ( USARTx -- USARTx_GTPR ) $18 + ; \ Guard Time and Prescaler Register

: bset! ( addr set|reset bitno -- )
  swap
  0<> swap      \ a -1|0 bitno
  1 swap lshift \ a -1|0 1<<bitno
  -rot          \ 1<<bn a 0|-1
  if
    bis!
  else
    bic!
  then
;



: rcc.en.usart2
  1 2 lshift RCC_APB2ENR bis!  \ Enable PA CLOCK
  1 17 lshift RCC_APB1ENR bis! \ Enable USART2 Clock
  2 GPIOA 1 gpio.af.set        \ Enable GPIO AF Out function
;

: usart2.int.enable \ Line 38
  1 38 32 - lshift NVIC_ISER1 bis! \ Enable 38-th line
;

: rcc.en.usart3
  1 3 lshift RCC_APB2ENR bis!  \ Enable PB CLOCK
  1 18 lshift RCC_APB1ENR bis! \ Enable USART2 Clock
;

: usart.cr1.pr ( 0|1 USARTx -- USARTx_CR1 0|1)
  USART.CR1 swap
;


: usart.ue.set ( 0|1 USARTx -- ) \ Set/Reset USART function
  usart.cr1.pr
  13 bset!
;

: usart.tcie.set ( 0|1 USARTx -- ) \ Set/Reset USART int on TC=1
  usart.cr1.pr
  6 bset!
;

: usart.rxneie.set ( 0|1 USARTx -- ) \ Set/Reset USART int on ORE=1 or RXNE=1
  usart.cr1.pr
  5 bset!
;

: usart.te.set ( 0|1 USARTx -- ) \ Set/Reset USART Transmit Enable
  usart.cr1.pr
  3 bset!
;

: usart.re.set ( 0|1 USARTx -- ) \ Set/Reset USART Receive Enable
  usart.cr1.pr
  2 bset!
;

: usart.comm.set ( 0|1 USARTx ) \ Setup bidirectional communications on USARTx
  2dup usart.ue.set
  2dup usart.te.set
  usart.re.set
;

: tb@ ( bitno addr -- T|F )
  swap 1 swap lshift swap
  bit@
;

: usart.sr.get ( bitno USARTx -- T|F )
  USART.SR tb@
;

: usart.sr.cts ( USARTx -- T|F ) \ Clear To Send detected.
  9 swap usart.sr.get
;

: usart.sr.lbd ( USARTx -- T|F ) \ Line Break Detected.
  8 swap usart.sr.get
;

: usart.sr.txe ( USARTx -- T|F ) \ Transmission buffer is empty?
  7 swap usart.sr.get
;

: usart.sr.tc ( USARTx -- T|F ) \ Transmission complete?
  6 swap usart.sr.get
;

: usart.sr.rxne ( USARTx -- T|F ) \ Is the Received byte has been sent to Data Register?
  5 swap usart.sr.get
;

: usart.sr.idle ( USARTx -- T|F ) \ Received IDLE byte.
  4 swap usart.sr.get
;

: usart.sr.ore ( USARTx -- T|F ) \ Overrun detected during receiving.
  3 swap usart.sr.get
;

: usart.sr.ne ( USARTx -- T|F ) \ Noise error.
  2 swap usart.sr.get
;

: usart.sr.fe ( USARTx -- T|F ) \ Framing error.
  1 swap usart.sr.get
;

: usart.sr.pe ( USARTx -- T|F ) \ Parity Error.
  0 swap usart.sr.get
;


: usart2.int.handler
  led.red.on
;

: ct \ COMM test
  rcc.en.usart2   \ Enable peripheral clock.
  USART2
  \ GPIO A2 A3 one in one out (alternate function)
  dup true swap usart.te.set \ Switch on the USART2
  dup true swap usart.re.set \ Switch on the USART2
  dup USART.BRR $45 swap ! \ Set 115207 Bit rate
  dup true swap usart.ue.set \ Switch on the USART2
  \ dup true swap usart.tcie.set \ Int on TC=1
  \ dup true swap usart.rxneie.set \ Int on RXNE=1
  drop
  ['] usart2.int.handler irq-usart2 ! \ Set interrupt handler
  \ usart2.int.enable          \ Int line 38
;

: usart.send ( char USARTx -- )
  dup   >r    \ char USARTx  \ R USARTx
  USART.DR !
  begin
    \ key? if leave then
    R@ usart.sr.tc \ tc != 1
  until
  rdrop
;


\ \ TESTING Stuff

\ $44444444 variable test
