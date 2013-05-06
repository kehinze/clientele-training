Table Module
============

Table Module looks like a Domain Model, the vital difference is that a Domain Model has one instance of a class for each row in the database whereas a Table Module has only one instance for the entire table.

A Table Module is in many ways a middle ground between a Transaction Script and a Domain Model. Organizing the domain logic around tables rather than straight procedures provides more structure and makes it easier to find and remove duplication. However, you can't use a number of the techniques that a Domain Model uses for finer grained structure of the logic, such as inheritance, strategies, and other OO patterns.

## Homework
- What are some of the strengths of the Table Module pattern?
- What are some of the weaknesses of the Table Module pattern?
- When would we consider Table Module?

