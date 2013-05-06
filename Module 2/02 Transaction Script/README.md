Transaction Script
==================

The simplest approach to storing domain logic is the Transaction Script. A Transaction Script is essentially a procedure that takes the input from the presentation, processes it with validations and calculations, stores data in the database, and invokes any operations from other systems. It then replies with more data to the presentation, perhaps doing more calculation to help organize and format the reply. 

The fundamental organization is of a single procedure for each action that a user might want to do. Hence, we can think of this pattern as being a script for an action, or business transaction. It doesn't have to be a single inline procedure of code. Pieces get separated into subroutines, and these subroutines can be shared between different Transaction Scripts. However, the driving force is still that of a procedure for each action, so a retailing system might have Transaction Scripts for checkout, for adding something to the shopping cart, for displaying delivery status, and so on.

## Homework
- What are some of the strengths of the Transaction Script pattern?
- What are some of the weaknesses of the Transaction Script pattern?
- When would we consider Transaction Script?

