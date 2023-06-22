# Inference Engine

This is a group assignment for university, and the goal is to implement an inference engine for propositional logic. The inference engine will be able to determine whether a given query can be inferred from a given knowledge base using various algorithms.

## Known Bugs

- The Backwards Chaining algorithm does not have support for test files with multiple of the same symbol, it will cause a stack overflow.

## Status

**COMPLETED!**

## TODO List

- ~~Implement Forwards Chaining~~
- ~~Add support for general KB to parser and TT~~
- ~~Clean up the code~~
- ~~Add support for queries that are not just a singular character (test_GeneralKB2.txt)~~
- ~~Convert the project to .NET 48 for the tutors.~~
- ~~Do the last couple parts of the submission document.~~

## Implementation

The software can be implemented using Java, Python, or C++/C#, with C# being our choice. The implementation will be tested on a standard Microsoft Windows 10 system. The assignment work should ideally be tested on a lab computer if possible.

## TT, FC, and BC Inference Engine

The inference engine supports three algorithms: Truth Table Checking (TT), Backward Chaining (BC), and Forward Chaining (FC). The TT algorithm works with all types of knowledge bases, while BC and FC are specifically designed for Horn-form knowledge bases. Given a knowledge base in Horn form (using the TELL keyword) and a query that represents a proposition symbol (using the ASK keyword), the software will determine whether the query can be logically inferred from the knowledge base using one of the three algorithms: TT, FC, or BC.

## File Format

The problems to be solved are stored in simple text files, containing both the knowledge base and the query. The knowledge base section follows the TELL keyword and consists of Horn clauses separated by semicolons. The query section follows the ASK keyword and consists of a single proposition symbol.

For example, a test file (e.g., test1.txt) could have the following content:

```javascript
TELL
p2=> p3; p3 => p1; c => e; b&e => f; f&g => h; p1=>d; p1&p3 => c; a; b; p2;

ASK
d
```

## Command Line Operation

The software operates through a command-line interface to facilitate batch testing. This can be achieved by using a simple .bat (batch) file if required. The program will be executed with the following command:

```
iengine method filename
```

- The iengine represents the executable file or the .bat (batch) file that calls the program.
- The method parameter can be TT (for Truth Table checking), FC (for Forward Chaining), or BC (for Backward Chaining), indicating the algorithm to be used.
- The filename parameter specifies the path to the text file containing the problem.

For example:

```
iengine FC test1.txt
```

## Output

The program will provide an output in the form of YES or NO, indicating whether the query can be logically inferred from the knowledge base. Additional information will be included based on the algorithm used:

- If the method is TT and the answer is YES, it will be followed by a colon (:) and the number of models satisfying the knowledge base.
- If the method is FC or BC and the answer is YES, it will be followed by a colon (:) and the list of propositional symbols entailed from the knowledge base during the execution of the algorithm.

For example, running the program with method TT on the provided test1.txt file will produce the following output:

```
YES: 3
```

When running the program with method FC on the same test1.txt file, it will produce the following output:

```less
> YES: a, b, p2, p3, p1, d
```

Please note that the specific order of the entailed propositional symbols may vary, as long as the results are correct.

In conclusion, this project involves the development of an inference engine for propositional logic. The implemented software supports three algorithms: Truth Table Checking (TT), Forward Chaining (FC), and Backward Chaining (BC). By providing a knowledge base and a query, the software can determine whether the query can be logically inferred from the knowledge base using the selected algorithm.

The software operates through a command-line interface, accepting input files in a specific format and producing output indicating the logical entailment of the query. The project aims to demonstrate understanding and proficiency in propositional logic inference algorithms.
