# MatchMaker

## Description
*MatchMaker* performs reiterative resampling procedures to produce random distributions of data. This program creates random pairings of dynamic data (such as data obtained from human behaviour) for a specifiable number of iterations, to create a random null distribution. The randoming procedure that *MatchMaker* uses to get to the null distribution is intriquitely specified, in order to maintain random integrity. This distribution can then be used as a baseline to compare true, observed data in order to statistically determine whether the structure in true data differs significantly from random.

## Application
In psychological research, an important question is how the behaviour of one group of humans compares to the behaviour of a different group of humans. However, statistically, any observed difference must be compared with differences between randomly selected measures of behaviour. *MatchMaker* allows for the creation of this random distribution in order to determine whether their observed human behaviour is truly different from random and therefore, meaningful. *MatchMaker* was used to create random distributions in Chapter 4 (in preparation for publication) of the doctoral dissertation, [***Back and Forth: Prediction and Interactive Alignment in Conversation***](https://qspace.library.queensu.ca/handle/1974/15375), authored by [**Nida Latif**](https://nlatif.wordpress.com/) in 2017.

## Development
*MatchMaker* is a *Windows Forms Application* developed entirely in *C#*. The user selects a folder in which all the files are located (a sample selection of such files are included in *MatchMaker* project files). Starting with the first file, each value in it is paired up with another random value from a different random file. This process is repeated for all values in every file in the selected folder. Once all values in every file is paired up, that concludes one trial. *MatchMaker* is able to repeat this process for the predetermined number of trials the experiment needs to run.

There are two files of results that are required to be printed. One is a spreadsheet detailing the average similarity and standard distribution of each individual trial, called the master file. The second file is a spreadsheet document detailing every random pairing for a randomly selected trial in the experiment. *MatchMaker* is able to accomplish this by using the *Microsoft Excel 16.0 Object Library* to create intriquite Excel spreadsheet documents programatically.

### Built With
* _Programming Language_: **C#** 
* _IDE_: **Visual Studio 2017**

### Author
[**Abdul Sadiq**](https://github.com/LedMetal)

### License
This project is licensed under the MIT License - see the LICENSE.md file for details

### Acknowledgments
[**Nida Latif**](https://nlatif.wordpress.com/) - Thank you for allowing my inclusion in your research!
