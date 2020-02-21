#!/bin/bash

DELAY=75
DEV=/dev/ttyUSB0

picocom --imap lfcrlf -b 115200 -s "ascii-xfr -s -l $DELAY -n" $DEV $*
# picocom --imap lfcrlf -b 9600 -s "ascii-xfr -s -l $DELAY -n" $DEV $*
