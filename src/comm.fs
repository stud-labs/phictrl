\ COMMUNICATION USART1,2,3

reset

$40013800 constant USART1
$40004400 constant USART2 \ FIXME: Check DOC
$40004800 constant USART3 \ FIXME: Check DOC



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

: usart.cr1.pr ( 0|1 USARTx -- USARTx_CR1 0|1)
  USART.CR1 swap
;


: usart.ue.set ( 0|1 USARTx -- ) \ Set/Reset USART function
  usart.cr1.pr
  13 bset!
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

: ct \ COMM test
  USART2
  dup true swap usart.ue.set \ Switch on the USART2
  dup USART.BRR $45 swap ! \ Set 115207 Bit rate
  drop
;


: usart.send ( char USARTx -- )
  dup   >r    \ char USARTx  \ R USARTx
  USART.DR !
  \ begin
    \ key? if leave then
  R@ usart.sr.tc
  \ until
  rdrop
;


\ \ TESTING Stuff

\ $44444444 variable test
