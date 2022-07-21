# TextAdventureScripting
A program that interprets formatted files into simple text adventure games.

# About
This program is inspired by [Ink](https://www.inklestudios.com/ink/), a much better narrative scripting language.
It is a simple scripting language that allows creating choose-your-own-story style text adventures.

An example adventure might be as follows:

    // Comment

    $ SECTION1

    - You enter a section titled “Section1”.
      For some reason, you are presented with a choice to make. Which option do you choose?
    >> Option A[] Led you deeper into an options tree! Oh no!
    >>> Option A.1[] went to a new branch. ~
    >>> Option A.2[] went in a circle and back to this branch.

    -- How do you get out of the second branch of the options tree?
    >>> Huh??[], you ask in confusion. Nothing changes.
    >>> [Climb down] You climb down the tree. There’s something at the bottom! ~

    >> Option B @SECTION2
    >> Option C ~ 

    - You reach a new prompt to deal with... What’ll you do?
    >> [Win the game!] You try to ‘win the game’, but you do not win the game. Do you even know how?
    >> [Sit and wait.] You sit and wait. That worked! Now you’re free! @END
    >> [Scream angrily.] You yell out into the void. Nothing happens.

    $ SECTION2

    Section 2 has nothing of interest. The game is over. @END
