\ COMMUNICATION USART1,2,3

reset

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

\ // Initialize and configure UART peripheral with specified baudrate
\ // input:
\ //   baudrate - UART speed (bits/s)
\ void UARTx_Init(USART_TypeDef* USARTx, uint32_t baudrate) {
\ 	GPIO_InitTypeDef PORT;

\ 	PORT.GPIO_Mode  = GPIO_Mode_AF;
\ 	PORT.GPIO_Speed = GPIO_Speed_40MHz;
\ 	PORT.GPIO_OType = GPIO_OType_PP;
\ 	PORT.GPIO_PuPd  = GPIO_PuPd_UP;

\ 	if (USARTx == USART1) {
\ 		RCC_AHBPeriphClockCmd(USART1_PORT_PERIPH,ENABLE);
\ 		RCC_APB2PeriphClockCmd(USART1_PORT_APB,ENABLE);
\ 		PORT.GPIO_Pin = USART1_TX_PIN;
\ 		GPIO_Init(USART1_GPIO_PORT,&PORT);
\ 		PORT.GPIO_Pin = USART1_RX_PIN;
\ 		GPIO_Init(USART1_GPIO_PORT,&PORT);
\ 		GPIO_PinAFConfig(USART1_GPIO_PORT,USART1_TX_PIN_SRC,USART1_GPIO_AF);
\ 		GPIO_PinAFConfig(USART1_GPIO_PORT,USART1_RX_PIN_SRC,USART1_GPIO_AF);
\ 	} else if (USARTx == USART2) {
\ 		RCC_AHBPeriphClockCmd(USART2_PORT_PERIPH,ENABLE);
\ 		RCC_APB1PeriphClockCmd(USART2_PORT_APB,ENABLE);
\ 		PORT.GPIO_Pin = USART2_TX_PIN;
\ 		GPIO_Init(USART2_GPIO_PORT,&PORT);
\ 		PORT.GPIO_Pin = USART2_RX_PIN;
\ 		GPIO_Init(USART2_GPIO_PORT,&PORT);
\ 		GPIO_PinAFConfig(USART2_GPIO_PORT,USART2_TX_PIN_SRC,USART2_GPIO_AF);
\ 		GPIO_PinAFConfig(USART2_GPIO_PORT,USART2_RX_PIN_SRC,USART2_GPIO_AF);
\ 	} else if (USARTx == USART3) {
\ 		RCC_AHBPeriphClockCmd(USART3_PORT_PERIPH,ENABLE);
\ 		RCC_APB1PeriphClockCmd(USART3_PORT_APB,ENABLE);
\ 		PORT.GPIO_Pin = USART3_TX_PIN;
\ 		GPIO_Init(USART3_GPIO_PORT,&PORT);
\ 		PORT.GPIO_Pin = USART3_RX_PIN;
\ 		GPIO_Init(USART3_GPIO_PORT,&PORT);
\ 		GPIO_PinAFConfig(USART3_GPIO_PORT,USART3_TX_PIN_SRC,USART3_GPIO_AF);
\ 		GPIO_PinAFConfig(USART3_GPIO_PORT,USART3_RX_PIN_SRC,USART3_GPIO_AF);
\ 	}

\ 	// Configure the USART: 1 stop bit (STOP[13:12] = 00)
\ 	USARTx->CR2 &= ~(USART_CR2_STOP);

\ 	// Configure the USART: 8-bit frame, no parity check, TX and RX enabled
\ 	USARTx->CR1 &= ~(USART_CR1_M | USART_CR1_PCE | USART_CR1_PS | USART_CR1_TE | USART_CR1_RE);
\ 	USARTx->CR1 |= USART_CR1_TE | USART_CR1_RE; // Transmitter and receiver enabled

\ 	// Configure the USART: CTS and RTS hardware flow control disabled
\ 	USARTx->CR3 &= ~(USART_CR3_CTSE | USART_CR3_RTSE);

\ 	// Configure USART port at given speed
\ 	UARTx_SetSpeed(USARTx,baudrate);

\ 	// Enable USART
\ 	USARTx->CR1 |= USART_CR1_UE;
\ }

: ct \ COMM test
  rcc.en.usart2   \ Enable peripheral clock.
  USART2
  dup true swap usart.ue.set \ Switch on the USART2
  \ GPIO A2 A3 one in one out (alternate function)
  dup true swap usart.te.set \ Switch on the USART2
  dup true swap usart.re.set \ Switch on the USART2
  dup USART.BRR $45 swap ! \ Set 115207 Bit rate
  dup true swap usart.ue.set \ Switch on the USART2
  drop
;


\ // Send single character to UART
\ // input:
\ //   USARTx - pointer to the USART port (USART1, USART2, etc.)
\ //   ch - character to send
\ void UART_SendChar(USART_TypeDef* USARTx, char ch) {
\ 	USARTx->DR = ch;
\ 	while (!(USARTx->SR & USART_SR_TC)); // wait for "Transmission Complete" flag cleared
\ }

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
