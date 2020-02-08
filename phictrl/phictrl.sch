EESchema Schematic File Version 4
EELAYER 30 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title ""
Date ""
Rev ""
Comp ""
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L Device:R R1
U 1 1 5E3BEBF4
P 9500 1050
F 0 "R1" V 9600 850 50  0000 C CNN
F 1 "150" V 9600 1050 50  0000 C CNN
F 2 "Resistor_SMD:R_0805_2012Metric_Pad1.15x1.40mm_HandSolder" V 9430 1050 50  0001 C CNN
F 3 "~" H 9500 1050 50  0001 C CNN
	1    9500 1050
	0    1    1    0   
$EndComp
$Comp
L Device:R R2
U 1 1 5E3BF0BE
P 9500 1250
F 0 "R2" V 9600 1050 50  0000 C CNN
F 1 "100" V 9600 1250 50  0000 C CNN
F 2 "Resistor_SMD:R_0805_2012Metric_Pad1.15x1.40mm_HandSolder" V 9430 1250 50  0001 C CNN
F 3 "~" H 9500 1250 50  0001 C CNN
	1    9500 1250
	0    1    1    0   
$EndComp
$Comp
L Device:R R3
U 1 1 5E3BF258
P 9500 1450
F 0 "R3" V 9600 1250 50  0000 C CNN
F 1 "100" V 9600 1450 50  0000 C CNN
F 2 "Resistor_SMD:R_0805_2012Metric_Pad1.15x1.40mm_HandSolder" V 9430 1450 50  0001 C CNN
F 3 "~" H 9500 1450 50  0001 C CNN
	1    9500 1450
	0    1    1    0   
$EndComp
Wire Wire Line
	9200 1050 9350 1050
Wire Wire Line
	9200 1250 9350 1250
Wire Wire Line
	9200 1450 9350 1450
$Comp
L power:GND #PWR0101
U 1 1 5E3BFFF9
P 8650 1250
F 0 "#PWR0101" H 8650 1000 50  0001 C CNN
F 1 "GND" H 8655 1077 50  0000 C CNN
F 2 "" H 8650 1250 50  0001 C CNN
F 3 "" H 8650 1250 50  0001 C CNN
	1    8650 1250
	1    0    0    -1  
$EndComp
Wire Wire Line
	9650 1250 9700 1250
Wire Wire Line
	9700 1250 9700 1150
Wire Wire Line
	9650 1450 9750 1450
Wire Wire Line
	9750 1450 9750 1250
Wire Wire Line
	8650 1250 8800 1250
$Comp
L Connector:Conn_01x04_Female J1
U 1 1 5E3C1ED4
P 10650 1150
F 0 "J1" H 10678 1126 50  0000 L CNN
F 1 "F-RGB" H 10678 1035 50  0000 L CNN
F 2 "Connector_Wire:SolderWirePad_1x04_P3.81mm_Drill1mm" H 10650 1150 50  0001 C CNN
F 3 "~" H 10650 1150 50  0001 C CNN
	1    10650 1150
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0102
U 1 1 5E3C26EC
P 9850 1400
F 0 "#PWR0102" H 9850 1150 50  0001 C CNN
F 1 "GND" H 9855 1227 50  0000 C CNN
F 2 "" H 9850 1400 50  0001 C CNN
F 3 "" H 9850 1400 50  0001 C CNN
	1    9850 1400
	1    0    0    -1  
$EndComp
Wire Wire Line
	9850 1350 9850 1400
Entry Wire Line
	10000 1050 10100 1150
Entry Wire Line
	10000 1150 10100 1250
Entry Wire Line
	10000 1250 10100 1350
Entry Wire Line
	10000 1350 10100 1450
Wire Wire Line
	9650 1050 10000 1050
Wire Wire Line
	9700 1150 10000 1150
Wire Wire Line
	9750 1250 10000 1250
Wire Wire Line
	9850 1350 10000 1350
Entry Wire Line
	10100 1150 10200 1050
Entry Wire Line
	10100 1250 10200 1150
Entry Wire Line
	10100 1350 10200 1250
Entry Wire Line
	10100 1450 10200 1350
Text Label 9750 1050 0    50   ~ 0
led-r
Text Label 9750 1150 0    50   ~ 0
led-g
Text Label 9750 1250 0    50   ~ 0
led-b
Text Label 9850 1350 0    50   ~ 0
gnd
Text Label 10200 1050 0    50   ~ 0
led-r
Wire Wire Line
	10200 1050 10450 1050
Wire Wire Line
	10200 1150 10450 1150
Wire Wire Line
	10200 1250 10450 1250
Wire Wire Line
	10200 1350 10450 1350
Text Label 10200 1250 0    50   ~ 0
led-g
Text Label 10200 1150 0    50   ~ 0
led-b
Text Label 10200 1350 0    50   ~ 0
gnd
$Comp
L Connector:Conn_01x04_Female J3
U 1 1 5E3CF70F
P 1450 2200
F 0 "J3" H 1342 1775 50  0000 C CNN
F 1 "J-PRG" H 1342 1866 50  0000 C CNN
F 2 "Connector_PinHeader_2.54mm:PinHeader_1x04_P2.54mm_Vertical" H 1450 2200 50  0001 C CNN
F 3 "~" H 1450 2200 50  0001 C CNN
	1    1450 2200
	-1   0    0    1   
$EndComp
Text Label 1700 2300 0    50   ~ 0
gnd
Text Label 1700 2000 0    50   ~ 0
vdd
Text Label 1700 2100 0    50   ~ 0
prg-tx
Text Label 1700 2200 0    50   ~ 0
prg-rx
Wire Wire Line
	1650 2000 1950 2000
Wire Wire Line
	1650 2100 1950 2100
Wire Wire Line
	1650 2200 1950 2200
Wire Wire Line
	1650 2300 1950 2300
$Comp
L MCU_ST_STM32F1:STM32F103C8Tx U1
U 1 1 5E3C6C25
P 5150 2450
F 0 "U1" H 5100 2600 50  0000 C CNN
F 1 "STM32F103C8Tx" H 5100 2450 50  0000 C CNN
F 2 "Package_QFP:LQFP-48_7x7mm_P0.5mm" H 4550 1050 50  0001 R CNN
F 3 "http://www.st.com/st-web-ui/static/active/en/resource/technical/document/datasheet/CD00161566.pdf" H 5150 2450 50  0001 C CNN
	1    5150 2450
	1    0    0    -1  
$EndComp
Text Label 4150 2850 0    50   ~ 0
prg-tx
Text Label 4150 2950 0    50   ~ 0
prg-rx
Wire Wire Line
	4100 2950 4450 2950
Wire Wire Line
	4100 2850 4450 2850
Wire Wire Line
	4450 3250 4100 3250
Wire Wire Line
	4450 3350 4100 3350
Text Label 4150 3250 0    50   ~ 0
rs-tx
Text Label 4150 3350 0    50   ~ 0
rs-rx
Text Label 5850 2450 0    50   ~ 0
bt-tx
Text Label 5850 2550 0    50   ~ 0
bt-rx
Wire Wire Line
	5750 2450 6100 2450
Wire Wire Line
	5750 2550 6100 2550
Text Label 4150 3550 0    50   ~ 0
bt-cts
Text Label 4150 3650 0    50   ~ 0
bt-rts
Text Label 5850 3050 0    50   ~ 0
led-r
Text Label 5850 3150 0    50   ~ 0
led-g
Text Label 5850 3250 0    50   ~ 0
led-b
Wire Wire Line
	5750 3050 6100 3050
Wire Wire Line
	5750 3150 6100 3150
Wire Wire Line
	5750 3250 6100 3250
Text Label 5850 2250 0    50   ~ 0
wake
$Comp
L power:VDD #PWR0103
U 1 1 5E3DE55C
P 5050 700
F 0 "#PWR0103" H 5050 550 50  0001 C CNN
F 1 "VDD" H 5067 873 50  0000 C CNN
F 2 "" H 5050 700 50  0001 C CNN
F 3 "" H 5050 700 50  0001 C CNN
	1    5050 700 
	1    0    0    -1  
$EndComp
Wire Wire Line
	5050 700  5150 700 
Wire Wire Line
	5250 700  5250 950 
Wire Wire Line
	5150 700  5150 950 
Connection ~ 5150 700 
Wire Wire Line
	5150 700  5250 700 
Wire Wire Line
	5050 700  5050 950 
Connection ~ 5050 700 
Text Label 5150 700  0    50   ~ 0
vdd
$Comp
L power:GND #PWR0104
U 1 1 5E3E0E45
P 4950 4050
F 0 "#PWR0104" H 4950 3800 50  0001 C CNN
F 1 "GND" H 4955 3877 50  0000 C CNN
F 2 "" H 4950 4050 50  0001 C CNN
F 3 "" H 4950 4050 50  0001 C CNN
	1    4950 4050
	1    0    0    -1  
$EndComp
Wire Wire Line
	4950 4050 4950 4000
Wire Wire Line
	4950 4000 5050 4000
Wire Wire Line
	5050 4000 5050 3950
Connection ~ 4950 4000
Wire Wire Line
	4950 4000 4950 3950
Wire Wire Line
	5050 4000 5150 4000
Wire Wire Line
	5150 4000 5150 3950
Connection ~ 5050 4000
$Comp
L Connector:Conn_01x04_Female J4
U 1 1 5E3E3BC8
P 1450 2850
F 0 "J4" H 1342 2425 50  0000 C CNN
F 1 "J-RS232" H 1342 2516 50  0000 C CNN
F 2 "Connector_PinHeader_2.54mm:PinHeader_1x04_P2.54mm_Vertical" H 1450 2850 50  0001 C CNN
F 3 "~" H 1450 2850 50  0001 C CNN
	1    1450 2850
	-1   0    0    1   
$EndComp
Text Label 1750 2950 0    50   ~ 0
gnd
Text Label 1750 2850 0    50   ~ 0
rs-rx
Text Label 1750 2750 0    50   ~ 0
rs-tx
Text Label 1750 2650 0    50   ~ 0
vdd
Wire Wire Line
	1650 2650 1950 2650
Wire Wire Line
	1650 2750 1950 2750
Wire Wire Line
	1650 2850 1950 2850
Wire Wire Line
	1650 2950 1950 2950
$Comp
L Connector:Conn_01x06_Female J5
U 1 1 5E3EA5BC
P 1450 3700
F 0 "J5" H 1342 3175 50  0000 C CNN
F 1 "J-BT" H 1342 3266 50  0000 C CNN
F 2 "Connector_PinHeader_2.54mm:PinHeader_1x06_P2.54mm_Vertical" H 1450 3700 50  0001 C CNN
F 3 "~" H 1450 3700 50  0001 C CNN
	1    1450 3700
	-1   0    0    1   
$EndComp
Text Label 1750 3800 0    50   ~ 0
gnd
Text Label 1750 3700 0    50   ~ 0
bt-rx
Text Label 1750 3600 0    50   ~ 0
bt-tx
Text Label 1750 3500 0    50   ~ 0
vdd
Wire Wire Line
	1650 3500 1950 3500
Wire Wire Line
	1650 3600 1950 3600
Wire Wire Line
	1650 3700 1950 3700
Wire Wire Line
	1650 3800 1950 3800
$Comp
L Connector:Conn_01x03_Female J2
U 1 1 5E3EDE56
P 1450 4450
F 0 "J2" H 1342 4125 50  0000 C CNN
F 1 "J-IR" H 1342 4216 50  0000 C CNN
F 2 "Connector_PinHeader_2.54mm:PinHeader_1x03_P2.54mm_Vertical" H 1450 4450 50  0001 C CNN
F 3 "~" H 1450 4450 50  0001 C CNN
	1    1450 4450
	-1   0    0    1   
$EndComp
Text Label 1750 4550 0    50   ~ 0
gnd
Text Label 1750 4350 0    50   ~ 0
vdd
Text Label 1750 4450 0    50   ~ 0
prg-rx
Wire Wire Line
	1650 4350 1950 4350
Wire Wire Line
	1650 4450 1950 4450
Wire Wire Line
	1650 4550 1950 4550
Wire Wire Line
	1650 3900 1950 3900
Wire Wire Line
	1650 3400 1950 3400
Text Label 1750 3400 0    50   ~ 0
bt-ready
Text Label 1750 3900 0    50   ~ 0
bt-reset
Text Label 3750 3550 0    50   ~ 0
bt-ready
Text Label 3750 3650 0    50   ~ 0
bt-reset
Wire Wire Line
	3700 3550 4450 3550
Wire Wire Line
	3700 3650 4450 3650
Text Label 6150 2250 0    50   ~ 0
bt-ready
Text Label 6600 2250 0    50   ~ 0
prg-tx
Wire Wire Line
	5750 2250 6900 2250
Wire Bus Line
	10100 1050 10100 1650
$Comp
L Device:LED_RCBG D1
U 1 1 5E3D3ECB
P 9000 1250
F 0 "D1" H 9000 1747 50  0000 C CNN
F 1 "LED_RCBG" H 9000 1656 50  0000 C CNN
F 2 "LED_THT:LED_D5.0mm-4_RGB" H 9000 1200 50  0001 C CNN
F 3 "~" H 9000 1200 50  0001 C CNN
	1    9000 1250
	1    0    0    -1  
$EndComp
Text Label 9250 1450 0    50   ~ 0
G
Text Label 9250 1250 0    50   ~ 0
B
Text Label 9250 1050 0    50   ~ 0
R
$EndSCHEMATC
