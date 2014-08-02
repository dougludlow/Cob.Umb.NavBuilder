Cob.Umb.NavBuilder
==================

Umbraco NavBuilder package for building large or complex navigation.

Usage
-----

    @{
        var menu = new NavBuilder(Model.Content.Id)
            .MaxLevel(2)
            .IncludeHome(true)
            .ShowAll(true);

        @Html.Raw(menu.Build())
    }

Example
-------

**Content Structure**

    Home
     | About
     | Contact
       | Location


**Code**

    @{
        // Create a new NavBuilder, passing in the current node's id
        var menu = new NavBuilder(Model.Content.Id)
            // Supply options via the fluent api
            .MaxLevel(2)
            .IncludeHome(true)
            .ShowAll(true);

        // Build the menu and output the html
        @Html.Raw(menu.Build())
    }

**Output**

    <ul>
        <li><a href="/">Home</a></li>
        <li><a href="/about/">About</a></li>
        <li>
            <a href="/contact/">Contact</a>
            <ul>
                <li><a href="/contact/location/">Location</a></li>
            </ul>
        </li>
    </ul>

Options
-------

(Coming soon...)