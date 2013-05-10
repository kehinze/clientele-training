Domain Model
============

With a Domain Model we build a model of our domain which, at least on a first approximation, is organized primarily around the nouns in the domain. Thus, a leasing system would have classes for lease, asset, and so forth. The logic for handling validations and calculations would be placed into this domain model, so shipment object might contain the logic to calculate the shipping charge for a delivery. There might still be routines for calculating a bill, but such a procedure would quickly delegate to a Domain Model method.

## Homework
- What are some of the strengths of the Domain Model pattern?
- What are some of the weaknesses of the Domain Model pattern?
- When would we consider Table Module?

