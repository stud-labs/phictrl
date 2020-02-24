true variable DEC.PREV
0 variable NEC.DECODE
true variable NEC.CODE
true variable NEC.STATE
48 constant NEC.LAST.STATE

: nec.state- ( -- NEC.STATE )
  NEC.STATE @ dup 1- NEC.STATE !
;

: nec.code.add ( n -- )
  NEC.DECODE @ 1 lshift or
  NEC.DECODE !
;

: nec.code.1
  1 nec.code.add
;

: nec.code.0
  0 nec.code.add
;

: nec.dec.down ( ms us -- )
  NEC.STATE @ true =
  ." \"
  if
    ." 48"
    NEC.LAST.STATE NEC.STATE ! \ Start decoding
    0 NEC.DECODE ! \ Initial value
    2drop
  else
    \ ." --"
    nec.state- drop
    \ 44 > \ preamble
    \ if
    \   2drop
    \ else \ decoding data
      swap \ us ms
      1 >=
      if
        ." 1"
        nec.code.1
      else
        ." 0"
        nec.code.0
      then
      drop \ -ms
    \ then
  then
  \ ." |"
;

: nec.dec.up ( ms us -- )
  nec.state- drop \ TODO: Check state sequence
  ." /"
  2drop
;

: nec.dec ( ms ns up|down -- )
  \ up = true, down = false
  if \ up
    nec.dec.up
  else
    nec.dec.down
  then
;


: s.d false nec.dec ;
: s.u true nec.dec ;

: nec.reset
  true DEC.PREV !
  0 NEC.DECODE !
  true NEC.STATE !
;


: nec.decode
  \ depth 3 / 0 do . . . ." |" loop cr
  ." CODE:"
  NEC.DECODE @
  dup NEC.CODE !
  dup hex.
  binary . decimal cr
  ." STATE:"
  NEC.STATE @ . cr
  ." STACK:" .s cr
  nec.reset
;


\ 3 2133 s.d 1 5609 s.u 1 4745 s.d 0 5857 s.u 0 3982 s.d
\ 0 5816 s.u 0 4273 s.d 0 5614 s.u 1 4543 s.d 0 5857 s.u
\ 0 4783 s.d 0 5816 s.u 0 5276 s.d 0 5857 s.u 0 5076 s.d
\ 0 5655 s.u 1 5114 s.d 0 5857 s.u 0 4742 s.d 0 5816 s.u
\ 0 5050 s.d 0 5816 s.u 0 5317 s.d 0 5614 s.u 1 5078 s.d
\ 1 5820 s.u 0 4454 s.d 0 5816 s.u 0 5009 s.d 0 5816 s.u
\ 0 5276 s.d 0 5857 s.u 1 4313 s.d 0 5857 s.u 0 4249 s.d
\ 0 5816 s.u 0 5276 s.d 0 5857 s.u 0 4807 s.d 0 5614 s.u
\ 1 4537 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u 0 5276 s.d
\ 0 5857 s.u 0 4850 s.d 0 5614 s.u 1 5380 s.d 0 6062 s.u nec.decode

\ bye

\
3 1870 s.d 0 5816 s.u
1 4540 s.d 1 5861 s.u 0 4249 s.d 0 5857 s.u
0 4516 s.d 0 5857 s.u 0 4314 s.d 1 5695 s.u
1 5013 s.d 0 5816 s.u 0 4783 s.d 0 5857 s.u
0 5317 s.d 0 5816 s.u 0 5075 s.d 0 5655 s.u
1 5116 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u
0 5276 s.d 0 5857 s.u 0 5050 s.d 0 5614 s.u
2 3879 s.d 0 5816 s.u 0 4742 s.d 0 5816 s.u
0 4783 s.d 0 5816 s.u 2 3874 s.d 0 5857 s.u
1 3580 s.d 0 5857 s.u 0 4516 s.d 0 5655 s.u
1 4848 s.d 1 5819 s.u 0 5009 s.d 0 5816 s.u
0 4742 s.d 0 5816 s.u 1 3868 s.d 0 5614 s.u nec.decode
\ 100010001001101001
\ 100010001001101001
1272 6 s.d nec.decode
8 6534 s.u nec.decode
\          46         45         44         43
3 2139 s.d 0 5857 s.u 0 4314 s.d 1 5660 s.u 1 4520 s.d
0 5857 s.u 0 4249 s.d 0 5816 s.u 0 4275 s.d 0 5614 s.u
1 5075 s.d 0 5939 s.u 0 5050 s.d 0 5816 s.u 0 4783 s.d
0 5816 s.u 0 5276 s.d 0 5652 s.u 0 5115 s.d 1 5778 s.u
0 5048 s.d 0 5816 s.u 0 4783 s.d 0 5816 s.u 0 5050 s.d
0 5816 s.u 0 4581 s.d 1 5655 s.u 1 5013 s.d 0 5816 s.u
0 4208 s.d 0 5816 s.u 1 3647 s.d 0 5654 s.u 2 4078 s.d
0 5816 s.u 0 4783 s.d 0 5857 s.u 1 3646 s.d 0 5655 s.u
1 5198 s.d 0 5816 s.u 0 4783 s.d 0 5857 s.u 1 4115 s.d
0 5816 s.u nec.decode
605 3172 s.d nec.decode
7 1457 s.u nec.decode

\ bye
\          46         45         44         43
4 2345 s.d 0 5816 s.u 0 3941 s.d 0 5857 s.u 0 4314 s.d
1 5653 s.u 1 4520 s.d 0 5857 s.u 0 4249 s.d 0 5816 s.u
0 5276 s.d 0 5857 s.u 1 4846 s.d 1 5818 s.u 0 5214 s.d
0 5816 s.u 0 4742 s.d 0 5816 s.u 0 5317 s.d 0 5816 s.u
0 5074 s.d 0 5655 s.u 1 5158 s.d 0 5816 s.u 0 4783 s.d
0 5816 s.u 0 4516 s.d 0 5857 s.u 0 4314 s.d 1 5779 s.u
0 4210 s.d 0 5816 s.u 0 5050 s.d 0 5816 s.u 1 3915 s.d
0 5614 s.u 1 5197 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u
0 5009 s.d 0 5816 s.u 0 5276 s.d 0 5655 s.u 1 5113 s.d
0 5820 s.u 1 3544 s.d 0 5857 s.u nec.decode
555 6804 s.d nec.decode
8 1666 s.u nec.decode

828 5009 s.d nec.decode
8 1751 s.u nec.decode
4 2142 s.d 0 6062 s.u 0 3982 s.d 0 5816 s.u 0 4516 s.d
0 5816 s.u 1 4542 s.d 1 5819 s.u 0 4454 s.d 0 5816 s.u
0 5050 s.d 0 5857 s.u 0 5050 s.d 0 5656 s.u 1 5116 s.d
1 5820 s.u 0 5091 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u
0 5276 s.d 0 5816 s.u 0 4807 s.d 1 5611 s.u 1 5279 s.d
0 5816 s.u 0 4742 s.d 0 5816 s.u 1 4077 s.d 0 5816 s.u
2 3874 s.d 0 5816 s.u 0 3982 s.d 0 5857 s.u 0 4516 s.d
0 5655 s.u 1 5114 s.d 1 5820 s.u 1 4013 s.d 0 5816 s.u
1 3809 s.d 0 5614 s.u 1 4846 s.d 1 5860 s.u 0 5091 s.d
0 5816 s.u nec.decode
747 273 s.d nec.decode
8 7703 s.u nec.decode
4 2338 s.d 0 5816 s.u 0 3941 s.d 0 5857 s.u 0 4516 s.d
0 5656 s.u 1 4581 s.d 0 5939 s.u 0 4249 s.d 0 5857 s.u
0 5050 s.d 0 5816 s.u 0 5074 s.d 0 5655 s.u 1 5110 s.d
0 5939 s.u 0 4742 s.d 0 5816 s.u 0 5050 s.d 0 5816 s.u
0 5317 s.d 0 5614 s.u 0 5074 s.d 1 5820 s.u 0 5255 s.d
0 5857 s.u 1 3581 s.d 0 5857 s.u 0 4849 s.d 0 5614 s.u
2 4136 s.d 0 5857 s.u 0 3982 s.d 0 5816 s.u 1 3873 s.d
0 5655 s.u 1 4437 s.d 0 5816 s.u 0 4742 s.d 0 5816 s.u
1 4079 s.d 0 5614 s.u 1 5072 s.d 0 5860 s.u 0 4783 s.d
0 5816 s.u nec.decode
1764 8051 s.d nec.decode
7 3102 s.u nec.decode
4 2103 s.d 1 5819 s.u 0 4249 s.d 0 5857 s.u 0 4516 s.d
0 5857 s.u 0 4582 s.d 1 5655 s.u 1 4519 s.d 0 5857 s.u
0 4783 s.d 0 5816 s.u 0 5009 s.d 0 5816 s.u 0 5074 s.d
1 5656 s.u 1 5049 s.d 0 5857 s.u 0 5050 s.d 0 5816 s.u
0 5050 s.d 0 5816 s.u 0 5113 s.d 0 5655 s.u 1 5112 s.d
0 5816 s.u 0 5009 s.d 0 5816 s.u 0 4208 s.d 0 5857 s.u
0 5115 s.d 0 5615 s.u 1 5115 s.d 0 5857 s.u 0 4249 s.d
0 5816 s.u 0 4475 s.d 0 5816 s.u 0 4807 s.d 1 5655 s.u
0 5522 s.d 0 5816 s.u 0 4208 s.d 0 5816 s.u 0 5050 s.d
0 5816 s.u 0 5115 s.d 1 5656 s.u 1 5279 s.d 0 5857 s.u nec.decode
666 7195 s.d nec.decode
7 1733 s.u nec.decode
4 2334 s.d 0 5816 s.u 0 3941 s.d 0 5857 s.u 0 4516 s.d
0 5656 s.u 1 4578 s.d 0 5816 s.u 0 4208 s.d 0 5857 s.u
0 5050 s.d 0 5816 s.u 0 5070 s.d 0 5655 s.u 1 5115 s.d
0 5857 s.u 0 4742 s.d 0 5816 s.u 0 5009 s.d 0 5816 s.u
0 5276 s.d 0 5610 s.u 1 4848 s.d 1 5818 s.u 0 5050 s.d
0 5816 s.u 0 4742 s.d 0 5816 s.u 1 3911 s.d 0 5614 s.u
1 5073 s.d 0 5898 s.u 0 4249 s.d 0 5816 s.u 0 4475 s.d
0 5816 s.u 2 3872 s.d 0 5857 s.u 0 4249 s.d 0 5857 s.u
0 5050 s.d 0 5816 s.u 0 5070 s.d 0 5656 s.u 1 5114 s.d
0 5857 s.u 0 4742 s.d 0 5857 s.u nec.decode
38 6541 s.d nec.decode
8 78 s.u 2 936 s.d 0 5816 s.u nec.decode
1019 7773 s.d nec.decode
8 3094 s.u nec.decode
3 2133 s.d 1 5609 s.u 1 4745 s.d 0 5857 s.u 0 3982 s.d
0 5816 s.u 0 4273 s.d 0 5614 s.u 1 4543 s.d 0 5857 s.u
0 4783 s.d 0 5816 s.u 0 5276 s.d 0 5857 s.u 0 5076 s.d
0 5655 s.u 1 5114 s.d 0 5857 s.u 0 4742 s.d 0 5816 s.u
0 5050 s.d 0 5816 s.u 0 5317 s.d 0 5614 s.u 1 5078 s.d
1 5820 s.u 0 4454 s.d 0 5816 s.u 0 5009 s.d 0 5816 s.u
0 5276 s.d 0 5857 s.u 1 4313 s.d 0 5857 s.u 0 4249 s.d
0 5816 s.u 0 5276 s.d 0 5857 s.u 0 4807 s.d 0 5614 s.u
1 4537 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u 0 5276 s.d
0 5857 s.u 0 4850 s.d 0 5614 s.u 1 5380 s.d 0 6062 s.u nec.decode
645 5001 s.d nec.decode
7 1424 s.u nec.decode
4 2312 s.d 0 5857 s.u 0 4516 s.d 0 5857 s.u 0 4315 s.d
1 5778 s.u 1 4516 s.d 0 5816 s.u 0 3941 s.d 0 5857 s.u
0 5317 s.d 0 5656 s.u 1 5113 s.d 0 5817 s.u 0 4783 s.d
0 5816 s.u 0 5050 s.d 0 5816 s.u 0 5317 s.d 0 5816 s.u
0 5074 s.d 1 5650 s.u 1 5052 s.d 0 5857 s.u 0 4783 s.d
0 5857 s.u 0 4783 s.d 0 5816 s.u 2 3868 s.d 0 5857 s.u
0 4783 s.d 0 5816 s.u 0 4475 s.d 0 5816 s.u 2 3877 s.d
1 5860 s.u 0 4824 s.d 0 5816 s.u 0 4742 s.d 0 5857 s.u
0 4516 s.d 0 5655 s.u 1 5112 s.d 0 5818 s.u 0 5050 s.d
0 5816 s.u 0 4783 s.d 0 5857 s.u nec.decode
595 6853 s.d nec.decode
8 1652 s.u nec.decode
3 2137 s.d 0 5857 s.u 0 4314 s.d 0 5652 s.u 1 4519 s.d
0 5816 s.u 0 3941 s.d 0 5857 s.u 0 4313 s.d 0 5655 s.u
1 5382 s.d 0 5980 s.u 0 4742 s.d 0 0816 s.u 0 5009 s.d
0 5816 s.u 0 5276 s.d 0 5816 s.u 1 4808 s.d 1 5820 s.u
0 5132 s.d 0 5816 s.u 0 4742 s.d 0 5857 s.u 0 5317 s.d
0 5816 s.u 2 3879 s.d 0 6021 s.u 0 4742 s.d 0 5816 s.u
0 5009 s.d 0 5816 s.u 0 5276 s.d 0 5816 s.u 2 3872 s.d
0 5857 s.u 1 3582 s.d 0 5857 s.u 0 4313 s.d 0 5655 s.u
1 4582 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u 1 4076 s.d
0 5857 s.u nec.decode
38 6596 s.d nec.decode
8 7979 s.u 1 4178 s.d 0 5614 s.u nec.decode
1110 2174 s.d nec.decode
7 3026 s.u nec.decode
4 2266 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u 0 4273 s.d
1 5613 s.u 1 4478 s.d 0 5857 s.u 0 3982 s.d 0 5816 s.u
0 5276 s.d 0 5614 s.u 1 5072 s.d 0 6062 s.u 0 5050 s.d
0 5816 s.u 0 4742 s.d 0 5857 s.u 0 5317 s.d 0 5816 s.u
0 4848 s.d 1 5779 s.u 1 5012 s.d 0 5816 s.u 0 5009 s.d
0 5816 s.u 0 4475 s.d 0 5857 s.u 2 3911 s.d 0 5816 s.u
0 4208 s.d 0 5857 s.u 0 4516 s.d 0 5857 s.u 0 4848 s.d
1 5696 s.u 0 5319 s.d 0 5816 s.u 0 4742 s.d 0 5816 s.u
1 3849 s.d 0 5655 s.u 1 5112 s.d 1 5818 s.u 0 5009 s.d
0 5857 s.u 0 4783 s.d 0 5816 s.u nec.decode
38 6777 s.d nec.decode
8 8053 s.u 1 4113 s.d 0 5816 s.u nec.decode
489 6926 s.d nec.decode
8 1291 s.u nec.decode
3 2173 s.d 0 5816 s.u 1 4539 s.d 1 5818 s.u 0 4249 s.d
0 5857 s.u 0 4516 s.d 0 5857 s.u 0 4312 s.d 1 5655 s.u
0 5522 s.d 0 5816 s.u 0 4783 s.d 0 5816 s.u 0 5050 s.d
0 5816 s.u 0 5072 s.d 0 5653 s.u 1 5119 s.d 0 5816 s.u
0 4742 s.d 0 5857 s.u 0 5317 s.d 0 5816 s.u 0 5276 s.d
0 5653 s.u 1 4578 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u
0 5050 s.d 0 5816 s.u 0 5009 s.d 0 5612 s.u 1 4538 s.d
0 5857 s.u 1 3586 s.d 0 5816 s.u 0 5276 s.d 0 5857 s.u
1 4308 s.d 1 5861 s.u 1 3581 s.d 0 5857 s.u 0 5317 s.d
0 5857 s.u 0 5113 s.d 0 5612 s.u nec.decode
485 6360 s.d nec.decode

8 1548 s.u nec.decode
4 2097 s.d 0 6021 s.u 0 4208 s.d 0 5857 s.u 0 4516 s.d
0 5857 s.u 1 4312 s.d 1 5817 s.u 0 4413 s.d 0 5816 s.u
0 5050 s.d 0 5816 s.u 0 5276 s.d 0 5612 s.u 0 5072 s.d
1 5816 s.u 1 5008 s.d 0 5816 s.u 0 4783 s.d 0 5816 s.u
0 5276 s.d 0 5857 s.u 0 4846 s.d 0 5612 s.u 1 5277 s.d
0 5857 s.u 1 3583 s.d 0 5857 s.u 1 3644 s.d 0 5653 s.u
1 5237 s.d 0 5816 s.u 1 3540 s.d 0 5816 s.u 1 3644 s.d
0 5653 s.u 2 4034 s.d 0 5857 s.u 0 4208 s.d 0 5857 s.u
0 5050 s.d 0 5654 s.u 2 4177 s.d 0 5816 s.u nec.decode
870 4925 s.d nec.decode
8 1354 s.u nec.decode
4 2150 s.d 0 5857 s.u 0 4316 s.d 0 5653 s.u 1 4579 s.d
0 5857 s.u 0 4208 s.d 0 5857 s.u 0 4516 s.d 0 5857 s.u
1 4845 s.d 1 5816 s.u 0 5214 s.d 0 5857 s.u 0 4783 s.d
0 5857 s.u 0 5317 s.d 0 5816 s.u 0 5072 s.d 0 5653 s.u
1 5196 s.d 0 5816 s.u 0 4783 s.d 0 5857 s.u 0 5050 s.d
0 5816 s.u 1 3913 s.d 1 5654 s.u 0 4474 s.d 0 5857 s.u
0 3982 s.d 0 5857 s.u 0 5317 s.d 0 5653 s.u 2 3911 s.d
0 5816 s.u 1 3538 s.d 0 5816 s.u 0 5276 s.d 0 5613 s.u
1 5072 s.d 1 5861 s.u 0 4783 s.d 0 5857 s.u 1 3848 s.d
0 5857 s.u nec.decode
38 6653 s.d nec.decode
8 7721 s.u 1 4647 s.d 0 5612 s.u nec.decode
645 5166 s.d nec.decode
8 1660 s.u nec.decode
4 2125 s.d 0 5816 s.u 0 3941 s.d 0 5857 s.u 0 4516 s.d
0 5857 s.u 1 4579 s.d 1 5813 s.u 0 4208 s.d 0 5816 s.u
0 5050 s.d 0 5816 s.u 0 5276 s.d 0 5612 s.u 1 5069 s.d
0 6062 s.u 0 4783 s.d 0 5816 s.u 0 5050 s.d 0 5816 s.u
0 5276 s.d 0 5816 s.u 0 5113 s.d 1 5654 s.u 0 5522 s.d
0 5816 s.u 1 3314 s.d 0 5857 s.u 0 4579 s.d 0 5612 s.u
1 5076 s.d 0 5857 s.u 1 3581 s.d 0 5816 s.u 1 3601 s.d
0 5612 s.u 2 3951 s.d 0 5857 s.u 0 4783 s.d 0 5816 s.u
1 3873 s.d 0 5653 s.u 2 3907 s.d 0 5816 s.u nec.decode
685 7022 s.d nec.decode
7 1441 s.u nec.decode
4 2557 s.d 0 5816 s.u 0 4208 s.d 0 5857 s.u 0 4312 s.d
1 5653 s.u 1 4519 s.d 0 5816 s.u 0 4208 s.d 0 5857 s.u
0 5050 s.d 0 5654 s.u 1 5112 s.d 1 5857 s.u 0 5173 s.d
0 5816 s.u 0 4783 s.d 0 5816 s.u 0 5276 s.d 0 5816 s.u
0 4805 s.d 1 5612 s.u 1 5277 s.d 0 5857 s.u 0 4783 s.d
0 5816 s.u 1 3806 s.d 0 5617 s.u 2 3866 s.d 0 5857 s.u
0 4249 s.d 0 5857 s.u 0 5317 s.d 0 5816 s.u 2 3909 s.d
1 5817 s.u 1 3579 s.d 0 5816 s.u 1 3805 s.d 0 5653 s.u
1 5380 s.d 1 5818 s.u 1 3624 s.d 0 5816 s.u nec.decode
136300
