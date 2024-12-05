:- use_module(library(dcg/basics)).
:- use_module(library(pure_input)).
mul(R) --> `mul(`, integer(X), `,`, integer(Y), `)`, {R is X*Y}.
part1(0) --> [].
part1(R) --> string(_), mul(A), string(_), part1(B), {R is A+B}.
:- phrase_from_file(part1(X), 'input.txt'),
   write('Part 1:'), write(X), nl.

without(_) --> [].
without(Thing) --> Thing, {!, fail}.
without(Thing) --> [_], without(Thing).
do(0) --> [].
do(R) --> without(`don't()`), mul(A), !, do(B), {R is A+B}.
do(R) --> without(mul(_)), (`don't()`, !, dont(R)).
do(0) --> string(_), [].
dont(0) --> [].
dont(R) --> string(_), `do()`, !, do(R).
dont(0) --> string(_), [].
:- phrase_from_file(do(X), 'input.txt'),
   write('Part 2:'), write(X), nl.