% runs on SWI-Prolog / Gorkem Pacaci AoC2024 Day 13
:- use_module(library(dcg/basics)).
:- use_module(library(pure_input)).
:- use_module(library(clpfd)).

button(X,Y) --> "Button ",string(_),": X+", integer(X), ", Y+", integer(Y).
prize(X,Y) --> "Prize: X=", integer(X), ", Y=", integer(Y).
machine(a(Xa,Ya), b(Xb,Yb), prize(Xp, Yp)) --> button(Xa,Ya), blank,
                                               button(Xb,Yb), blank,
                                               prize(Xp,Yp).
file([]) --> [].
file([machine(A,B,P)|Ms]) --> machine(A,B,P), blanks, file(Ms).

main :- 
    phrase_from_file(file(X), 'input.txt'),
    maplist(minCost(0), X, MinCosts),
    sum_list(MinCosts, TotCost),
    format("Part 1, Total Cost: ~p~N", TotCost),
    maplist(minCost(10000000000000), X, MinCosts2),
    sum_list(MinCosts2, TotCost2),
    format("Part 2, Total Cost: ~p~N", TotCost2).

minCost(PrizeOffset, machine(a(Xa,Ya),b(Xb,Yb),prize(Xp,Yp)), Cost) :-
    A in 0..sup, B in 0..sup,
    Xp + PrizeOffset #= A * Xa + B * Xb,
    Yp + PrizeOffset #= A * Ya + B * Yb,
    Cost #= A * 3 + B,
    once(labeling([min(Cost)], [A,B,Cost])) -> true; Cost=0.

:- main.