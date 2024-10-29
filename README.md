<!-- PROJECT LOGO -->
<br />
<div align="center" id="readme-top">
  <a href="https://alramz.ae/">
    <img src="images/logo.png" alt="Logo" width="auto" height="150">
  </a>

  <h3 align="center"> Financial Data - Unit Tests </h3>

  <p align="center">
    A toolkit that showcases the testing of historical financial data through both the objective and subjective approach.
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installations">Installations</a></li>
        <li><a href="#running">Running the Project</a></li>
      </ul>
    </li>
    <li><a href="#license">License</a></li>
  </ol>
</details>


<!-- ABOUT THE PROJECT -->
## About The Project
This repository provides a comprehensive toolkit for testing historical financial data using both objective and subjective approaches. The objective tests focus on validating data integrity through concrete rules, such as ensuring the open price is higher than the low for that day and positive trading volumes, while the subjective tests compare data from multiple sources, selecting the most reliable one using metrics like Least Squared Error (LSE). With well-documented code snippets and examples, this repository serves as a valuable resource for developers and analysts looking to ensure the accuracy and quality of market data in financial models. Whether you're new to finance or an experienced professional, the toolkit simplifies the process of validating historical data.
<p id="about-the-project" align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With
<div ></div>
The project was built using the following frameworks/languages. Make sure to have the frameworks installed in order to run the project.
<br> <br>
<a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0">
    <img src="images/dotnet.png" alt="Logo" width="auto" height="120">
</a>
 <br>
<a href="https://dotnet.microsoft.com/en-us/languages/csharp">
    <img src="images/csharp.png" alt="Logo" width="auto" height="150">
  </a>


<p id="built-with" align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started
<div  id="getting-started"> </div>
This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

<h3> Installations </h3>
<div  id="installations"> </div>
<ol>
  <li> If you haven’t installed <b> Visual Studio 2022 </b>, download it from <a href = "https://visualstudio.microsoft.com/"> Microsoft's Visual Studio page </a>. </li>
  <li> Make sure you have <b><a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0">.NET 8</a> </b> installed if you haven't installed it already. </li>
  <li> In Visual Studio, Go to <b>File</b> > <b>Clone Repository</b>.</li>
  <li> Enter the GitHub URL of the repository (https://github.com/ramzqrd/data-testing) </li>
  <li> Click <b>Clone</b></li>
</ol>
<h3> Running the Project </h3>
<div  id="running"> </div>
<ol>
  <li> Once the solution is cloned, run the <b>Application</b> project to generate the files containing the data to be tested.</li>
  <li> After it completes running, you’ll find the various sample data files in the <b>Data</b> folder on your desktop. </li>
  <li> Go to <b>Test</b> > <b>Test Explorer</b> to view all the available unit tests (<b>Objective</b> & <b>Subjective</b>). </li>
  <li> Click on <b>Run</b> to run all tests. Otherwise, you can select individual tests to run.</li>
  <li> Once the test finishes running, you should be able to see the results of each test (<b>Pass</b> or <b>Fail</b>). </li>
</ol>
<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- License -->
## License
<div  id="license"> </div>
Distributed under the <b>MIT</b> License, a widely adopted, permissive open-source license that allows users to:
<ul> 
<li> Use the software for any purpose. </li>
<li> Modify the software. </li>
<li> Distribute copies of the software. </li>
<li> Merge into other projects. </li>
<li> Publish derivative works. </li>
<li> Sublicense or include in proprietary software. </li>
</ul>
Just make sure to include the original license and copyright notice with any distributed copy.
<p align="right">(<a href="#readme-top">back to top</a>)</p>
