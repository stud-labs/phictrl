#!/bin/bash

DELAY=200

picocom --imap lfcrlf -b 113200 -s "ascii-xfr -s -l $DELAY -n" /dev/rfcomm0 $*
