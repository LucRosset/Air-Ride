# Development Log

You can find the development log/diary in the _Development log_ folder!

# Coding format standard
This project obeys the following coding standard (for C#, other languages may vary):

## Indentation

- Indent using tabs (preferably, size your tab size to 2 spaces, although this should not influence the actual text files)
- Indent items inside scopes, arrays, lists, tupples etc. one level more than the opening level
- Scopes should be open with _{_ in a separate line, in the same indent level as the previous line
- Scopes should close with _}_ in a separate line, in the same intend level as the opening _{_

## Listing and line breaks

- Arrays, lists, tupples etc, when declared with their explicit content, may be broken into multiple lines. The same can be applied to function parameters
	- Open the parenthesis or bracket, then list each element in a new line, indented one level more than the opening bracket
	- Close the parenthesis or bracket in a new line, with the same indent level as the opening
	- You may continue writing code from the closing bracket
	- Commas go in the end of the lines, not at the beginning! We're not savages!
- Arithmetic operations and other "one-liners" may be broken down into more lines. Begin each line with the arithmetic operator and indent it one level from the starting line of that expression

Example:

```
someVar = MyFunction(
	param1,
	param2
) - Mathf.pi;
someOtherVar = (longNamedVariable * 2 + 1)
	/ anotherVariableWithALongName;
```

## Naming standard

- Use camelCase to name variables, constants, classes, functions etc.
- Classes, functions and methods must begin with a upper-case letter, variables and constants must begin with a lower-case letter
- Name references to Game Object components as close to their class' names as possible, but do not include the _2D_ posfix. Example: `private Collider2D myCollider;` Here, the variable was not simply called `collider` because there is a (deprecated) variable in MonoBehaviour already named that, which causes a warning to trigger. Thus, in those cases, add a "my" prefix to the name.

## Class permissions

- Make class variables private whenever possible, prefer serializing private variables to creating public ones. Except when the variable needs to be set externally, then public variables may be used.
- Create setters and getters for variables with one of the following options:

	`public <type> <varName> {public get; <private/public> set;}` (Prefered)

	or

	```
	[SerializeField] private <type> <varName> = <initVal>;

	public <type> <VarName> // upper-case in the first letter, this is a method!
	{
		get => <varName>;
		set => <varName> = value;
	}
	```

## Class structure

The order in which variables and methods should be declared in a class are the following:

1. Variables
2. Inherited method
3. Class' overritten methods
4. Class' own methods and IEnumerators

## Documentation

C# can be documented through markups in the comments.
This documentation can be accessed by some IDEs and editors such as VS Code to give the user information about types, parameters, descriptions etc. about classes, interfaces, methods and variables.

If a markup does not make sense (such as a return description for a void method), it can be ignored.
You may break lines between the opening and closing tags.

Please use the following markups in your code for (non-inherited) and public: classes, interfaces, methods and class variables.
The markup content must be following a three-slashes comment (`///`).

```
///<summary>Description of the class, interface, method, variable etc.</summary>
///<param name="parameter1">Description of a method's parameter.
///Add one per parameter in a method or function.</param>
///<return>Method's return description</return>
```

Another important feature in Unity are the [Attributes](https://docs.unity3d.com/Manual/Attributes.html). The most used are `SerializeField`, `Header()` and `Tooltip()`.
Use them to organize the script's interface with Unity editor's inspector.

## C# template file
The C# template for Unity can be found under the following folders:

- Windows: C:\Program Files\Unity\Hub\Editor\\<version\>\Editor\Data\Resources\ScriptTemplates\\**81-C# Script-NewBehaviourScript.cs.txt**
- Mac: /Applications/Unity/Editor/Data/Resources/ScriptTemplates/**81-C# Script-NewBehaviourScript.cs.txt**
- Mac (since 5.2.1f1): /Applications/Unity/Unity.app/Contents/Resources/ScriptTemplates/**81-C# Script-NewBehaviourScript.cs.txt**

Copy the **81-C# Script-NewBehaviourScript.cs.txt** file from this directory to the corresponding folder from your operating system.
I recomend renaming the default file or saving a backup, just in case, but that is not critical in any way.

Unity Hub will create a new template file for each new version of Unity you install.