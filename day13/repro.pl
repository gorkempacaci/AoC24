% runs on Sicstus / Gorkem Pacaci AoC2024 Day 13

:- use_module(library(clpfd)).
:- use_module(library(lists)).
				
minCost(Ax, Bx, P, A, B) :-
    A in 0..sup, B in 0..sup,
    P #= A*Ax + B*Bx,
    Cost #= A+B,
    labeling([minimize(Cost)], [A,B,Cost]) -> true; (A=nil, B=nil).

timeSolve((Ax, Bx, P)) :-
    statistics(runtime, [Start,_]),
    minCost(Ax, Bx, P, A, B),
    statistics(runtime, [Stop,_]),
    T is Stop-Start,
    fd_statistics(resumptions, RS),
    fd_statistics(entailments, ET),
    fd_statistics(prunings, PN),
    fd_statistics(backtracks, BT),
    fd_statistics(constraints, CT),
    format("Ax=~w, Bx=~w, P=~w. Solution: A=~w, B=~w~N", [Ax, Bx, P, A, B]),
    format("Time:~w, Resump.s:~w, Entlmnts:~w, Prunings:~w, Backtrack::~w, Constrs:~w ~N", [T, RS, ET, PN, BT, CT]).

% In the slow examples, Time increases from 0s to ~1500s, and Resumptions and Prunings increase x10^5
main :-
    write('Fast SAT examples'), nl,
    maplist(timeSolve, [(27, 60, 100003443), (32, 40, 100005040), (21, 59, 100013541)]), nl,

    write('Slow SAT example'), nl,
    maplist(timeSolve, [(48, 16, 100017648)]), nl,

    write('Fast UNSAT examples'), nl,
    maplist(timeSolve, [(32, 96, 10009632), (20,60, 100007540), (20,60, 100007540)]), nl,

    write('Slow UNSAT examples'), nl,
    maplist(timeSolve, [(18, 21, 100001860), (96, 16, 100005968), (45, 13, 100002438)]), nl,
    halt.

:- main.

%Output I get on Sicstus 4.7.1 and 4.9.0 (similar) on both MacOS and Windows (emulated on Parallels)
%Fast SAT examples
%Ax=27, Bx=60, P=100003443. Solution: A=9, B=1666720
%Time:0, Resump.s:148, Entlmnts:8, Prunings:157, Backtrack::1, Constrs:4 
%Ax=32, Bx=40, P=100005040. Solution: A=0, B=2500126
%Time:0, Resump.s:310, Entlmnts:8, Prunings:364, Backtrack::1, Constrs:4 
%Ax=21, Bx=59, P=100013541. Solution: A=19, B=1695138
%Time:0, Resump.s:235, Entlmnts:8, Prunings:214, Backtrack::1, Constrs:4 
%
%Slow SAT example
%Ax=48, Bx=16, P=100017648. Solution: A=2083701, B=0
%Time:2598, Resump.s:25004424, Entlmnts:8334812, Prunings:27088128, Backtrack::0, Constrs:4 
%
%Fast UNSAT examples
%Ax=32, Bx=96, P=9632. Solution: A=1, B=100
%Time:0, Resump.s:43, Entlmnts:8, Prunings:47, Backtrack::1, Constrs:4 
%Ax=20, Bx=60, P=100007540. Solution: A=1, B=1666792
%Time:0, Resump.s:82, Entlmnts:8, Prunings:95, Backtrack::1, Constrs:4 
%Ax=20, Bx=60, P=100007540. Solution: A=1, B=1666792
%Time:0, Resump.s:82, Entlmnts:8, Prunings:95, Backtrack::1, Constrs:4 
%
%Slow UNSAT examples
%Ax=18, Bx=21, P=100001860. Solution: A=nil, B=nil
%Time:1231, Resump.s:19047989, Entlmnts:2, Prunings:14285996, Backtrack::1, Constrs:3 
%Ax=96, Bx=16, P=100005968. Solution: A=1041728, B=5
%Time:1327, Resump.s:12500752, Entlmnts:4166920, Prunings:13542485, Backtrack::0, Constrs:4 
%Ax=45, Bx=13, P=100002438. Solution: A=2222266, B=36
%Time:1556, Resump.s:22564578, Entlmnts:683780, Prunings:16581572, Backtrack::0, Constrs:4 