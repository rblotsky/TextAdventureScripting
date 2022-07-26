# TextAdventureScripting (TAScript)
A program that interprets formatted files into simple text adventure games.

# About
This program is inspired by [Ink](https://www.inklestudios.com/ink/), a much better narrative scripting language.

TAScript is a simple scripting language that allows creating choose-your-own-story style text adventures.

An example adventure might be as follows:

```
// Comment

$ SECTION1 // This is a section header

- You enter a section titled “Section1”.
  For some reason, you are presented with a choice to make. Which option do you choose? // This is a prompt
>> Option A[] Led you deeper into an options tree! Oh no! // This is an option (that has sub-options!)
>>> Option A.1[] went to a new branch. // This block continues to the next Prompt ("How do you...")
>>> Option A.2[] went in a circle and back to this branch. ~ // This block returns to the previous prompt

-- How do you get out of the second branch of the options tree?
>>> Huh??[], you ask in confusion. Nothing changes. ~
>>> [Climb down]You climb down the tree. There’s something at the bottom!

>> Option B[]Goes to section 2! @SECTION2
>> Option C[]Lets you go to the next prompt! ~ 

- You reach a new prompt to deal with... What’ll you do?
>> [Win the game!]You try to ‘win the game’, but you do not win the game. Do you even know how? ~
>> [Sit and wait.]You sit and wait. That worked! Now you’re free! @END
>> [Scream angrily.]You yell out into the void. Nothing happens. ~

$ SECTION2

- Section 2 has nothing of interest. The game is over. @END

$ END
```

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
   By default, selecting an option will continue to the next prompt (`-`) on a lower indent level, unless it contains a reroute (`@`) or </b>
   a return (`~`).

## Comments
Comments are denoted with a `//` symbol. If a line contains a comment, *the entire rest of the line will be ignored*

## Indentation
As in Python, "indentation" is highly important here. However, "indents" are not considered actual tabs or whitespace, but the amount of</b>
indent characters (`-` or `>`) before the line.

Any Options ***must*** be 1 indent level ***higher*** than the previous block.</b>

## Return and Continue
By default, a block will *continue* to the next prompt on a lower indent level, within the same section. However, by placing a Return point (`~`) at the end of the block</b>
the execution will instead return to the previous prompt. This does *nothing* if there is no previous prompt.

Similarly, if a Return point is not used and there are no further prompts within the same section, execution will end.

## Special Sections & Reroutes
- A Reroute (`@`) redirects execution to the first prompt within a named section. If the section doesn't exist, or there are no prompts in it, execution stops.
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

*Note: This happens after parsing commands! If you add a square bracket within a command, it might not be used to split text.*

## Commands
Commands are special expressions that can be placed ONLY within Block text, and can perform a number of things. For example, the "RANDOM" command </b>
can be used to randomize what text is displayed somewhere.

Commands have a simple syntax: `{COMMAND_NAME:Variable,Variable,...,Variable}` </b>
Everything placed within `{` `}` is considered a command.

- `COMMAND_NAME` The name of the command to run. Different commands do different things. There is a list of valid command names </b>
   later in this document.
- `Variables`    Variables are the input given to the command. Each command might expect different variables, and a different amount of them. </b>
   Adding too few, too many, or invalid variables will be considered a syntax error and will prevent parsing the command.
   
## Valid Commands:
 Command Name | Variables     | Functionality 
--------------|---------------|---------------
 RANDOM</b>   | Text Value</br>| Gets replaced with a random value from the variables.
              | (can be given any amount of text values)|               


