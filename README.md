# TextAdventureScripting (TAScript)
A program that interprets formatted files into simple text adventure games.

# About
This program is inspired by [Ink](https://www.inklestudios.com/ink/), a much better narrative scripting language.

TAScript is a simple scripting language that allows creating choose-your-own-story style text adventures.

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
    >>> [Climb down]You climb down the tree. There’s something at the bottom! ~

    >> Option B[]Goes to section 2! @SECTION2
    >> Option C[]Lets you go to the next prompt! ~ 

    - You reach a new prompt to deal with... What’ll you do?
    >> [Win the game!]You try to ‘win the game’, but you do not win the game. Do you even know how?
    >> [Sit and wait.]You sit and wait. That worked! Now you’re free! @END
    >> [Scream angrily.]You yell out into the void. Nothing happens.

    $ SECTION2

    - Section 2 has nothing of interest. The game is over. @END

    $ END


# Writing TAScript (Text-Adventure-Script)
## Building Blocks
There are 3 main building blocks:
- Sections (Denoted by `$`):
    - Sections are named regions of script that can contain Prompts and Options.</b>
   You can use the `@` symbol to move the current execution point to the first prompt in that section. For example:</b>
 
   `@SECTION2` will display the first prompt following `$ SECTION2` in the script.
- Prompts  (Denoted by `-`)
    - Prompts are where the user is *prompted* for input. These are generally followed by 1 or more options.</b>
   Prompts are also very useful as "continue" points, were blocks ending with `~` will reroute to if there are no further options.</b>
- Options  (Denoted by `>`)
    - Options are placed following a prompt or another option *with 1 extra indent*. When displaying text, all text beginning with `>`</b>
   and indented one layer higher will be displayed as options the user can select.</b>
   By default, selecting an option will return the execution point to their last position, unless it contains a reroute (`@`) or </b>
   a continue (`~`).

## Comments
Comments are denoted with a `//` symbol. If a line contains a comment, *the entire rest of the line will be ignored*

## Indentation
As in Python, "indentation" is highly important here. However, "indents" are not considered actual tabs or whitespace, but the amount of</b>
indent characters (`-` or `>`) before the line.

Any Options ***must*** be 1 indent level ***higher*** than the previous block.</b>
If a continue (`~`) point is used, it will continue to the next Prompt (`-`) one or more levels *below* itself.

## Special Sections & Reroutes
- The `$ END` section is mandatory to be included at the end of your file. It allows rerouting to itself to complete the game.
- The `@END` reroute moves execution to the aforemention `$ END` section, finishing the game. 
* Note: If you are editing this program, that's a simplified explanation of how the `$ END` section works and why it is necessary.

## Multiline Blocks
Blocks can be written across multiple lines, and doing so will include the line break as part of the text.
A block finishes *whenever a new block starts*.

## Splitting Text
Text can contain special `[` and `]` characters to split it into multiple parts:
- The text *before* the brackets is displayed *always*
- The text *inside* the brackets is displayed only *as an option*
- The text *after* the brackets is displayed only *after selecting the block*




