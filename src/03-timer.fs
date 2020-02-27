\ Program Name: timer.fs
\ Date: 2016
\ Copyright 2017  t.porter <terry@tjporter.com.au>, licensed under the GPL
\ For Mecrisp-Stellaris by Matthias Koch
\ Chip: STM32F051
\ Board: STM32F0 Discovery Board
\ Terminal: e4thcom, Copyright (C) 2013-2017 Manfred Mahlow (GPL'd)  https://wiki.forth-ev.de/doku.php/en:projects:e4thcom
\ Clock: 8 Mhz using the internal STM32F051 RC clock, unless otherwise stated
\ All register names are CMSIS-SVD compliant
\ Note: gpio a,b,c,d,e, and uart1 are enabled by Mecrisp-Stellaris core as default.
\
\ This Program :
\ * measures elapsed time using the SYSTICK.
\ * Flashes the BLUE led every second by counting 1000 systicks
\ * A 1mS marker pulse is available on PB2
\
\ Registers Used:- As defined below
\ timer.fs is self contained, no other files are required
\ To be done: modify the code to run from Flash after a reboot.
\ ------------------------------------------------------------------------------------------------------

compiletoflash

 \ Define Systick memory mapping as its not in CMSIS-SVD
$E000E010 constant STK_CSR	\ SysTick control and status register. R/W reset value = $00000000
$E000E014 constant STK_RVR	\ SysTick reload value register. R/W reset value = 6000 (6MHz clock) for the STM32F0
$E000E018 constant STK_CVR	\ SysTick current value register. R/W value unknown
$E000E01C constant STK_CALIB	\ SysTick calibration value register. Read Only, $40001770 for the STM32F0

 \ 16000000 $E000E014 ! \ How many ticks between interrupts ?
 \        7 $E000E010 ! \ Enable the systick interrupt.

\ Variables
0 variable ms_counter		\ can count to 32 bits or -> $ffffffff u. =  4294967295 mS or 4294967 seconds, or 71582 minutes or 1193 hours.
0 variable USER-WORD       \ User procedure address called every 1ms with ms_counter stack value ( ms_counter -- ) if not zero.

: systick-handler
  ms_counter
  dup @
  1+ swap !
  USER-WORD @
  dup 0<>
  if
    ms_counter @
    swap
    execute
  else
    drop
  then
;

\ ENABLE-SYSTICK-INTERRUPT	    \ enable it last

: INIT-SYSTICK
  8080 STK_RVR !			\ systick calib for 1ms using internal 8mhz osc
  ['] systick-handler irq-systick !    \ This 'hooks' the systick-handler word (above) to the systick irq
  %111 STK_CSR bis!		    \ systick enable with interrupt
;


: delay ( delay-value -- elapsed-time )
  0 ms_counter !			    \ reset the ms_counter to zero
  0 DO LOOP			    \ the simple delay loop to be timed
  ." = "
  ms_counter @ .			    \ this will be updated by the systick-handler every systick and we just read it when the delay finishes
  ." milliseconds " cr
;


: stk.current ( -- 24bit-systick-value ) \ The current systick value
  STK_CVR @
;

: stk.reload.set ( 24bit-val -- ) \ Set systick reload value
  STK_RVR !
;

compiletoram


\ ~~~~~~~~~~ Screenshots ~~~~~~~~~~~~
\ 10000 delay = 6 milliseconds
\ 1000000 delay = 625 milliseconds
\ 10000000 delay = 6256 milliseconds
