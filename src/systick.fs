\ Program Name: systick.fs
\ Date: Wed 8 Jan 2020 13:13:57 AEDT
\ Copyright 2020 by t.j.porter <terry@tjporter.com.au>,  licensed under the GPLV3
\ For Mecrisp-Stellaris by Matthias Koch.
\ https://sourceforge.net/projects/mecrisp/
\ Standalone: no preloaded support files required
\
\ This Program: Interrupt driven STM32Fxxx Systick Timing Library
\
\ Note the (STK) is not a part of STM32F CMSIS-SVD
\ See: ST PM0215 Programming manual, page 85-91
\
\ --------------------------- Example 1ms-cal-value -------------------------\
\  8000   Standard Mecrisp-Stellaris using 8MHz internal RC clock
\  48000  STM32F0xx running at 48 MHz
\  72000  STM32F10x running at 72 MHz
\ ------------------------- Example Library Usage ---------------------------\
\
\  Systick must be initialised before use: "<1ms-cal-value> init.systick"
\
\ ---------------------------------------------------------------------------\
\ 
\  : 10s-delay cr	   \ Demo 10 second delay
\    ." 10 second delay starting, please wait ..." cr
\    10000 ms.delay   ( 10 seconds )
\    ." 10 second delay finished" cr
\  ; 
\   
\  : do.stuff ( -- )       \ blocking do-loop  
\    4800000 0 do loop	   \ time will vary depending on interrupts etc so
\  ;			   \ blocking do-loop are not very accurate
\    
\  : timeit cr		   \ measure the time taken to "do.stuff" above
\    ms.counter.reset
\    do.stuff
\    ." do.stuff took "
\    ms.print cr
\  ;
\  
\  10s-delay
\ 
\  timeit 
\  do.stuff took 184 ms
\
\ ---------------------------------------------------------------------------\

 \ compiletoflash
 compiletoram
 
 $E000E010   constant stk_csr     \ RW  SysTick control and status  
 $E000E014   constant stk_rvr     \ RW  SysTick reload value 
 $E000E018   constant stk_cvr     \ RW  SysTick current value 
 $E000E01C   constant stk_calib   \ RO  SysTick calibration value 

 0 variable ms.counter	\ 32 bits or -> $ffffffff u. = 4294967295 ms, 4294967 seconds,
			\ 71582 minutes, 19.88 Hrs
 
 : tickint ( -- )       \ tickint: sysTick exception request enable
   %010 stk_csr bis!    \ 1 = Counting down to zero asserts the SysTick exception request.
 ;
 
 : systick-handler ( -- )
   1 ms.counter +!
 ;
 
 : init.systick	( 1ms-cal-value -- )   \ init systick
   stk_rvr !			       \ systick calib for 1ms 
   %101 stk_csr bis!		       \ systick enable
   ['] systick-handler irq-systick !   \ 'hooks' the systick-handler word (above) to the systick interrupt
   tickint
 ;
 
 : ms.counter.reset ( -- )	       \ clear the ms.counter
   0 ms.counter !		       
 ;
 
 : ms.read ( -- )		       \ read elapsed ms since ms.counter.reset
   ms.counter @			       \ this will be updated by the systick-handler every systick
 ;
 
 : ms.print ( -- )		       \ print elapsed ms since ms.counter.rese
   ms.read . ." ms " cr
 ;
 
 : ms.reached? ( u -- yes = -1 | no = 0 )     \ TRUE when u reached
   ms.read u<
 ;
 
 : ms.delay ( u -- )  \ accurate  blocking delay, range is 1ms to 19.88 Hrs (32 bytes)
   ms.counter.reset
      begin
	 ms.read over u>=
      until
   drop
 ;
 
 48000 init.systick   \ Add to any existing init word to run at boot from Flash.
 




