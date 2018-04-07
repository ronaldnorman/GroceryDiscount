==============================================================
GroceryDiscounts Readme file
Author: Ronald Norman
Date: Sat March 3, 2017
Version: 1.0.0
==============================================================

OVERVIEW
GroceryCo is a shopping basket checkout software specializing in supporting multiple discount strategies on the item level.

DESIGN QUALITY ATTRIBUTES
Configarabiliy:
The system is highly configurable in three major areas:
1) The catalog SKUs and their discount schems are defined in an easy to read XML file.
2) The basket of items to be checked are also defined in a simple XML file
3) Other configurations such as business name, phone number, path to the catalog xml file, etc... are defined in the app config file.

Extensibility:
The system applies the OCP princple, multi-layered, and is open to extension and closed to change. The system is extensible in the following major areas:
1) Discount strategies. Discount strategies are encapsulated into their own classes each and behind interfaces for ease of swapping. It's easy to add more discount strategies to the catalog xml file and straight forward. Having each discount strategy in its own class makes them highly customziable without adding more complexity and downgrading readability.
2) Data Model/Storage. Right now the data model is XML files, however, the data access is enapsulated into its own factory where SQL or any other storage model can be added and swapped with a few lines of code.
3) Display (UI). There's seperation between the UI and the core engine. It's is easy to swap a different display than the console as the process of seperated from the presentation.
4) Standard data & coding interfaces. To suppor seperation of converns and loose coupling, I implemented stand-alone data entities whose main responsibility is to act as the data shuttle between different components and layers. Also, interfaces are defined to add flexibility.
5) Future features. The design has allocated placehodlers for future functionality such as emailing the receipt or sending it to a printer.

Readabiliy:
1) High commented with a lot of 'why' comments to provide team members the reason behine codes so they can make better decisions.
2) Seperation of concerns. Seperating responsibility into their own components and layers helps readers understand the purpose of each area easily so they can do their changes.
3) Minimum Repeating (DRY). There's high degree of factoring and minimum repeating of same code. Having said that, there's room for refactoring.
4) Project structure. The structure of the project files are organized in solution folders to help team membres quickly understand the major parts of the solution and where everything is.
5) Coding conventions. Coding convetions are highly conformed to from consistent use of casing to use long variable names to help understandability inline.

Testabiliy:
Seperation of concerns, especially the business layer from the UI helps testing components reperately.

Flexibility:
The discount strategies are implemented in the most flexible way to support many scenarios. For example, "buy one item get second item half off" also works for "buy 3 items get the fourth 10% off". Very flexible.

Performance
The system is designed with an eye towards high performance. For instance, the XML catalog could turn out to be a very large file, so I don't read it in-memory, instead, I stream it, then only load the sku section into memory. I measured roughly the performance and all sku's are processed in O(1) time.

Maintainability:
The system is highly maintainable with minimum effort because of the above quality attributes. 

DESIGN DECISIONS
1) XML data. The catalog and the basket are defined in XML. I decided on XML format because it strikes a balance between user-friendliness (a requirement) and ease/flexibility of parsing. Although it is not as friendly as a GUI (not a requirement), an average user should be able to open any of those xml files and make modifications. I made sure to desribe all names in long english format vs. ambiguous short-letters words.
2) XmlReader instead of XDocument. I opted to use the XmlReader for performance reasons as the XDocument loads the whole document in memory and the catalog xml file's limit is open. Large catalog files are highly supported.
3) Added qty. Although it wasn't required, I took the liberty to add Qty to basket items.
4) Used a few design patterns such as Singleton, Factory and Strategy to seperate concerns, support extensibility, and the other quality attributes of the system.

SETUP
1) Basket xml file. The system prompts the customer to input the absolute path to the basket xml file at runtime.
2) Catalog xml file. The catalog xml file defines the SKUs info and discount strategies for each SKU. The path to this file is defined in the App.Config file under the AppSettings section. Make sure to change it.

UNIT TESTS
Basic unit tests are included with solution. The design of the system leans itself towards testing from the outside without the UI. Unit tests has their own test basket xml files and catalog xml file.
The path to this file is in the App.Config file under the AppSettings section. Make sure you point to it.

LIMITATIONS
1) Single session and single location system

ASSUMPTIONS
1) An XSD will be added to validate the XML files at load time and minimize format errors introduced by user modifications.
2) Local environment. The system will be hosted in the place where it's running. So I didn't use Universal Date Time (UTC), and stuck with the DateTime.Now


==============================================================
