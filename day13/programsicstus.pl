% runs on Sicstus / Gorkem Pacaci AoC2024 Day 13

% === DCG STUFF FOR SICSTUS

:- use_module(library(clpfd)).
:- use_module(library(lists)).

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

minCost(PrizeOffset, machine(a(Xa,Ya),b(Xb,Yb),prize(Xp,Yp)), Cost) :-
    A in 0..sup, B in 0..sup,
    Xp + PrizeOffset #= A*Xa + B*Xb,
    Yp + PrizeOffset #= A*Ya + B*Yb,
    Cost #= A*3 + B,
    labeling([minimize(Cost)], [A,B,Cost]) -> true; Cost=0.

main :- 
    phrase_from_file(file(X), 'input.txt'),
    maplist(minCost(0), X, MinCosts),
    sumlist(MinCosts, TotCost),
    format("Part 1, Total Cost: ~p~N", TotCost),
    maplist(minCost(10000000), X, MinCosts2),
    sumlist(MinCosts2, TotCost2),
    format("Part 2, Total Cost: ~p~N", TotCost2),
    fd_statistics,
    halt.

:- main.