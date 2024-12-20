% runs on Sicstus / Gorkem Pacaci AoC2024 Day 13

% === DCG STUFF FOR SICSTUS

:- use_module(library(clpfd)).
:- use_module(library(lists)).
:- use_module(library(chr)).

% Fact: Only some examples trigger the lag, most take 0ms.

% Theory: maplist is slow. NOT. Used simple recursion.
% Theory: GC is triggered. NOT. Disabled GC.
% Slow on ARM? Same on Windows. 

:- set_prolog_flag(gc,off).
%:- chr_flag(toplevel_show_store, _, on).


read_file_to_string(File, String) :-
    open(File, read, Stream),
    read_all_chars(Stream, String),
    close(Stream).

read_all_chars(Stream, [C|CRest]) :- get_code(Stream, C0),
                                     \+at_end_of_stream(Stream),
				     C=C0,
				     read_all_chars(Stream, CRest).
read_all_chars(Stream, []) :- at_end_of_stream(Stream). 

phrase_from_file(Type, File) :-
    read_file_to_string(File, String),
    phrase(Type, String).

digit(D) --> [D], {D >= 48, D =< 57}.

natural_string([Ni|NRest]) --> digit(Ni), natural_string(NRest).
natural_string([Nz]) --> digit(Nz).

natural_number(Number) --> natural_string(NString), {number_codes(Number, NString)}.

integer(I) --> natural_number(I).
integer(Im) --> "-", natural_number(I), {Im is -I}. 

string([]) --> [].
string([C|Rest]) --> [C], string(Rest).

white --> [C], {memberchk(C, [9,11,12,32])}.
whites --> [].
whites --> white, whites.

blank --> [C], {memberchk(C, [9,10,11,12,13,32])}.
blanks --> [].
blanks --> blank, blanks.

buttona(X,Y) --> "Button A: X+", integer(X), ", Y+", integer(Y).
buttonb(X,Y) --> "Button B: X+", integer(X), ", Y+", integer(Y).
prize(X,Y) --> "Prize: X=", integer(X), ", Y=", integer(Y).
machine(a(Xa,Ya), b(Xb,Yb), prize(Xp, Yp)) --> buttona(Xa,Ya), blanks,
                                               buttonb(Xb,Yb), blanks,
                                               prize(Xp,Yp).
file([]) --> [].
file([machine(A,B,P)|Ms]) --> machine(A,B,P), blanks, file(Ms).

% === MODEL

minCost(PrizeOffset, machine(a(Xa,Ya),b(Xb,Yb),prize(Xp,Yp)), A, B, Cost) :-
    A in 0..sup, B in 0..sup,
    Xp + PrizeOffset #= A*Xa + B*Xb,
    Yp + PrizeOffset #= A*Ya + B*Yb,
    Cost #= A*3 + B,
    labeling([minimize(Cost)], [A,B,Cost]) -> true; Cost=0.

doAll(_, [], []).
doAll(PrizeOffset, [M|Machines], [C|Costs]) :-
    statistics(runtime, [Start,_]),
    minCost(PrizeOffset, M, A, B, C),
    statistics(runtime, [Stop,_]),
    T is Stop-Start,
    M=machine(a(Xa,Ya),b(Xb,Yb),prize(Xp,Yp)),
    fd_statistics(resumptions, RS),
    fd_statistics(entailments, ET),
    fd_statistics(prunings, PN),
    fd_statistics(backtracks, BT),
    fd_statistics(constraints, CT),
    format("~w;~w;~w;~w;~w;~w;~w;~w;~w;~w;~w;~w;~w;~w;~w~N", [Xa,Ya, Xb,Yb, Xp,Yp, A,B,C, T, RS,ET,PN,BT,CT]),
    doAll(PrizeOffset, Machines, Costs).

main :- 
    phrase_from_file(file(Xs), 'input.txt'),
    format("Done reading from the file.~N", []),
    doAll(0, Xs, MinCosts),
    sumlist(MinCosts, TotCost),
    format("Part 1, Total Cost: ~p~N", TotCost),
    doAll(10000000, Xs, MinCosts2),
    sumlist(MinCosts2, TotCost2),
    format("Part 2, Total Cost: ~p~N", TotCost2),
    fd_statistics,
    halt.

:- main.