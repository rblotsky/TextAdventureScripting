# TextAdventureScripting (TAScript)
A program that interprets formatted files into simple text adventure games.

# About
This program is inspired by [Ink](https://www.inklestudios.com/ink/), a much better narrative scripting language.

TAScript is a simple scripting language that allows creating choose-your-own-story style text adventures, however it can be used for a wider range
of uses. For example:

- Multiple choice quizzes
- Game dialogue trees
- etc.

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
>>> [Back to first prompt.] You go back to the first prompt! ~~ // This goes back to the first prompt because it uses two ~ symbols.
>>> [Climb down]You climb down the tree. There’s something at the bottom!

>> Option B[]Goes to section 2! @SECTION2
>> Option C[]Lets you go to the next prompt! ~ // This goes back to "You enter Section1"

- You reach a new prompt to deal with... What’ll you do?
>> {COND:Section2\=\0}[Win the game!]You try to ‘win the game’, but you do not win the game. Do you even know how? ~ 
   // This prompt is only displayed if Section 2 has not been visited, as specified by the Command at the beginning.
  
>> [Sit and wait.]You sit and wait. That worked! Now you’re free! @END
>> [Scream angrily.]You yell out into the void. Nothing happens. ~

$ SECTION2

- Section 2 has nothing of interest. The game is over. @END

$ END
```

# Using TAScript in a Program
TAScript has 3 main parts:
- Compiler: Compiles source files into ParsedBlocks, then into Runnable blocks.
- Runner: Uses a graph of Runnable blocks to run the logic behind a game.
- Controller: Interfaces with the Runner to display text and manage user input.

When integrating TAScript into a program, all you need to do is run the Compiler (using `Compiler.CompileGame()`) and then use a custom Controller script to manage user input.
Controller scripts follow a simple format:
```pseudo
while not finished:
  display prompt
  if options:
    display options
  
  get user input
  if options:
    select an option
    change active block to one returned by Runnable
  if not options:
    run continue
    change active block to one returned by Runnable
```

There is an example [Controller script](./TAScript/Controller/GameController.cs) included in this repository that can be used as reference.

# Writing TAScript (Text-Adventure-Script)
## Building Blocks
There are 3 main building blocks:
- Sections (Denoted by `$`):
    - Sections are named regions of script that can contain Prompts and Options.
   You can use the `@` symbol to move the current execution point to the first prompt in that section. For example:
 
      `@SECTION2` will display the first prompt following `$ SECTION2` in the script.
- Prompts  (Denoted by `-`)
    - Prompts are where the user is *prompted* for input. These are generally followed by 1 or more options.
   Prompts are also very useful as "continue" points, were blocks ending with `~` will reroute to if there are no further options.
- Options  (Denoted by `>`)
    - Options are placed following a prompt or another option *with 1 extra indent*. When displaying text, all text beginning with `>`
   and indented one layer higher will be displayed as options the user can select.
   By default, selecting an option will continue to the next prompt (`-`) on a lower indent level, unless it contains a reroute (`@`) or 
   a return (`~`).
   
##### One-Time Options
Options can have a `?` character placed at the front of the option (eg. `>>>> ? Text Here`) to make them only usable once. This means that regardless of how
many times the previous prompt is run, this option will only appear *before* it is selected, and after it is selected it will not longer be available.

## Comments
Comments are denoted with a `//` symbol. If a line contains a comment, *the entire rest of the line will be ignored*

## Indentation
As in Python, "indentation" is highly important here. However, "indents" are not considered actual tabs or whitespace, but the amount of
indent characters (`-` or `>`) before the line.

Any Options ***must*** be 1 indent level ***higher*** than the previous block.

## Return and Continue
By default, a block will *continue* to the next prompt on a lower indent level, within the same section. However, by placing a Return point (`~`) at the end of the block
the execution will instead return to the previous prompt. This does *nothing* if there is no previous prompt.
By adding more `~` characters, you can increase how far back the Return goes. For example:
 - `~` goes back to this option's prompt.
 - `~~` goes to the prompt that the last prompt was an option for.

Similarly, if a Return point is not used and there are no further prompts within the same section, execution will end.

## Special Sections & Reroutes
- A Reroute (`@`) redirects execution to the first prompt within a named section. If the section doesn't exist, or there are no prompts in it, execution stops.
- The `$ END` section is mandatory to be included at the end of your file. It allows rerouting to itself to complete the game.
- The `@END` reroute moves execution to the aforemention `$ END` section, finishing the game. 

*Note: If you are editing this program, that's a simplified explanation of how the `$ END` section works and why it is necessary.*

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
Commands are special expressions that can be placed ONLY within Block text, and can perform a number of things. For example, the "RANDOM" command 
can be used to randomize what text is displayed somewhere.

Commands have a simple syntax: `{COMMAND_NAME:Argument\Argument\...\Argument}` 
Everything placed within `{}` is considered a command.
Arguments are separated with a backslash `\` character.

- `COMMAND_NAME:` The name of the command to run. The colon `:` after COMMAND_NAME is mandatory, even if there are no arguments for the command.
- `Arguments`    Variables are the input given to the command. Each command might expect different variables, and a different amount of them.
   Adding too few, too many, or invalid variables will be considered a syntax error and will prevent parsing the command.
   
## Variables
Variables are stored within the game and used for Commands. Variable names are required to be text, excluding newlines, backslashes, and `{}[]`
characters. When a variable is first used, it is given a value of 0, and in subsequent uses the current value will be used.

Some Commands allow modifying variable names, such as ADD or SET.

#### Special Variables:

Some variables are reserved by the game. For example, Section names are set as variables, as well as all Tags used in text.
Section names are given a value equal to the number of times that section has been entered.
Tags are given a value equal to the number of times that tag has appeared.

Special variables can be modified by Commands, but it is important to remember that they are modified elsewhere outside of commands too.

## Valid Commands
Argument Descriptions:
- `Var`: Expects a variable name (variable names can be written similarly to Text arguments)
- `Text`: Expects text, all characters except newlines, backslashes, and `{}[]` are accepted.
- `Operator`: Expects an operator (>, <, =, !, >=, <=) that defines what operation to use for the expression.
- `Value`: Expects an integer value.
- `None`: Expects nothing.

#### Commands with Multiple Argument Lists: 

A command can have multiple types of expected arguments. For example, RAND can have either no arguments, or a list of text values. This means 
that a different functionality will run depending on which arguments are used. 

Possible functionalities for different overloads are listed in the following table.

 Command Name | Arguments       | Functionality 
--------------|-----------------|---------------
 RAND         | Text\Text...| Gets replaced with a random value from the variables. *Note: Not implemented*
 RAND         | None            | Randomly decides whether to display this option in the previous prompt. *Note: Not implemented*
 COND         | Var\Operator\ReqValue\SuccessText\FailureText | Displays SuccessText if the expression specified is true, FailureText if not.
 COND         | Var\Operator\ReqValue | Displays this option if the expression specified is true. *Note: If multiple COND commands are used, ALL of them must pass.*
 SET          | Var\Value       | Sets variable Var to value Value.
 ADD          | Var\Value       | Adds value Value to variable Var. 
 MIN          | Var\Value       | Sets the variable Var to the lower value of the 2: itself, and Value. *Note: Not implemented*
 MAX          | Var\Value       | Sets the variable Var to the higher value of the 2: itself, and Value. *Note: Not implemented*
 NOEMPTY      | None            | Removes all empty lines from the text in this block.
 PRINTVAR     | Var             | Prints the value of variable Var.
