\ Program Name: f103-diagnostics-image-blinky.fs
\ Date: Wed 5 Feb 2020 13:33:55 AEDT
\ Copyright 2020 by t.j.porter <terry@tjporter.com.au>, licensed under the GPLV2
\ For Mecrisp-Stellaris by Matthias Koch.
\ https://sourceforge.net/projects/mecrisp/
\ Board: Blue Pill with LED on PC-13
\ Designed to be used with STM32F103C8-DIAGNOSTICS.bin
\ https://sourceforge.net/projects/mecrisp-stellaris-folkdoc/files/STM32F103C8-DIAGNOSTICS.bin
\ All register names are CMSIS-SVD compliant
\ Note: gpio a,b,c,d,e, and uart1 are enabled by Mecrisp-Stellaris Core.
\ Standalone: no preloaded support files required
\
\ This Program : Blinks the Blue Pill LED on PC-13 
\
\ The Blue Pill LED anode is connected to +3.3v via a 510R resistor and
\ the Cathode to PC-13.
\ This INVERTS the normal operation so LED is on at bootup.
\ PC-13 = HIGH, LED = OFF
\ PC-13 = LOW, LED = ON
\ ---------------------------------------------------------------------------\
 compiletoram
 \ compiletoflash

 \ Configure PC-13 as Open Drain, OUTPUT.
 \ PC13->output	  \ See STM document RM008 page 172 0 1134
 \ 23 22  21 20
 \ CNF13  MODE13
 \ 0  1   1  1  
 %11 20 lshift GPIOC_CRH bis!	 \ GPIOC_CRH_MODE13  Output mode: 50MHz
 %01 22 lshift GPIOC_CRH bis!	 \ GPIOC_CRH_CNF13   Open Drain
 
 : led.off   %1  13 lshift GPIOC_BSRR bis! ;   
 : led.on  %1 13 lshift GPIOC_BRR bis! ;
 
 : blink
      10 0 do		\ blink 10 times, 1/2 second on, 1/2 second off.
	 led.on
	 500 ms.delay	\ use ms.delay Word included in STM32F103C8-DIAGNOSTICS.bin
	 led.off
	 500 ms.delay
      loop
 ;   

 blink

  






















