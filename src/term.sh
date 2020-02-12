#!/bin/bash

DELAY=75

picocom --imap lfcrlf -b 113200 -s "ascii-xfr -s -l $DELAY -n" /dev/rfcomm0 $*

