## Regression Short-Term Stability API
made for Bharat Petroleum Corporation
This API aims to performing regression analysis on a given dataset and calculating the shortterm stability and shelf life of Reference Materials. The API reads a JSON file containing data points, conducts linear regression analysis, and returns various statistical metrics and insights. 
•	Framework: ASP.NET Core 
•	Version: .NET 8.0
•	Environment: Visual Studio Professional 2022
•	Language: C#
•	Packages Used: MathNet.Numerics, NewtonSoft.Json, Swashbuckle.Microsoft.Core 
Formula Table: 
 ![Screenshot 2024-06-26 122250](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/8c989e24-83d0-4dea-b36f-b2a09c5df48d)



## Linear Regression in Excel 
# Regression Statistics: 
This section tells us how well the calculated linear regression equation fits our source data. 

 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/8eea491a-1a44-4283-ac8d-af15f8841477)

Multiple R:  It is the Correlation Coefficient that measures the strength of a linear relationship between two variables. The correlation coefficient can be any value between 1 and 1, and its absolute value indicates the relationship strength. The larger the absolute value, the stronger the relationship:
o	1 means a strong positive relationship
o	1 means a strong negative relationship
o	0 means no relationship at all

R Square: It is the Coefficient of Determination, which is used as an indicator of the goodness of fit. It shows how many points fall on the regression line. The R2 value is calculated from the total sum of squares, more precisely, it is the sum of the squared deviations of the original data from the mean.

Adjusted R Square: It is the R square adjusted for the number of independent variables in the model. Us will want to use this value instead of R square for multiple regression analysis.

Standard Error: It is another goodnessoffit measure that shows the precision of usr regression analysis  the smaller the number, the more certain us can be about usr regression equation. While R2 represents the percentage of the dependent variables variance that is explained by the model, Standard Error is an absolute measure that shows the average distance that the data points fall from the regression line.

Observations: It is simply the number of observations in our model.

# ANOVA:
The second section of the output is Analysis of Variance (ANOVA).
 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/99684174-77a0-4995-a929-a26c031a53c7)

In statistics, oneway analysis of variance is a technique to compare whether two or more samples' means are significantly different. This analysis of variance technique requires a numeric response variable "Y" (densities) and a single explanatory variable "X" (week number), hence "oneway".
It splits the sum of squares into individual components that give information about the levels of variability within our regression model.
o	df is the number of degrees of freedom associated with the sources of variance.
o	SS is the sum of squares. The smaller the Residual SS compared with the Total SS, the better usr model fits the data.
o	MS is the mean square.
o	F is the F statistic or Ftest for the null hypothesis. It is used to test the overall significance of the model.
o	Significance F is the Pvalue of F.

# Coefficients:
This section provides specific information about the components of our analysis. 
 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/0bb0e060-e9b0-4134-bb1f-1b465a75f193)

The most useful component in this section is coefficients. It enables us to build a linear regression equation in Excel: y = bx + a 
o	Coefficient: Gives us the least squares estimate.
o	Standard Error: the least squares estimate of the standard error.
o	T Statistic: The T Statistic for the null hypothesis vs. the alternate hypothesis. 
o	P Value: Gives us the pvalue for the hypothesis test.
o	Lower 95%: The lower boundary for the confidence interval.
o	Upper 95%: The upper boundary for the confidence interval.
*Confidence Interval is taken standard at 95%.


## Code Explanation:
## Packages: 
 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/fb37be2d-cd96-4c19-a57c-8753e94690ba)

o	MathNet.Numerics: A library for advanced numerical computations.
o	Microsoft.AspNetCore.Mvc: ASP.NET Core MVC framework for creating web APIs.
o	Newtonsoft.Json: A library for JSON serialization and deserialization.
o	System.IO and System.Linq: Standard libraries for input/output operations and LINQ queries.

## Controller: RegressionController.cs
 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/4429f056-3650-458e-92d5-d8acabffa62d)

o	Route and ApiController Attributes: Specifies the route for the API and identifies the class as an API controller.

 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/439344cb-8eab-4053-af6d-6662362b5c9a)

o	HttpPost Attribute: Indicates that this method handles POST requests.
o	Analyze Method: Handles file upload, performs regression analysis, and returns results.

 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/73c09f3f-94d8-42e3-a87d-49be8310bc41)

o	File Check: Validates if the uploaded file is null or empty.
o	File Reading: Reads the contents of the uploaded JSON file.

 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/d4f84bad-fc70-4fb3-8452-fef129d8f990)

o	Data Deserialization: Converts JSON data into a list of `RegressionData` objects.
o	Data Validation: Checks if the deserialized data is null or empty.

 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/818c07f5-2bd3-456d-a0b8-a7e7c462f31c)

o	Extracting Data: Retrieves periods (weeks) and densities from the deserialized data.
o	Performing Regression: Fits a linear regression model using `SimpleRegression.Fit`.
o	Predictions: Generates predictions using the regression model.
o	RSquared Calculation: Computes the coefficient of determination (R²) to evaluate the model.

 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/4252795d-57b1-4e32-b100-10ecdeb43537)

o	Average Density: Computes the average density.
o	Observations Check: Ensures there are at least 3 data points for analysis.
o	Mean Calculation: Calculates the mean of the densities.
o	Sum of Squares: Computes the total sum of squares.
o	Sum of Residuals: Computes the sum of squared residuals.
o	Standard Error: Calculates the standard error of the regression.
o	Adjusted RSquared: Adjusts the R² value for the number of predictors.
o	Multiple R: Computes the correlation coefficient.


![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/c8cfe806-b389-4877-9f29-3b1517ef4dae)

 
o	Sum of Squares: Computes total, regression, and residual sums of squares.
o	Degrees of Freedom: Calculates degrees of freedom for total, regression, and residual.
o	Mean Squares: Computes mean squares for regression and residual.
o	FValue: Calculates the Fstatistic.
o	Significance F: Computes the pvalue for the Fstatistic.

 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/f9ce7262-55df-4524-907a-e04d98f60da1)

o	Intercept and Slope: Retrieves the regression coefficients.
o	Standard Errors: Calculates the standard errors of the intercept and slope.
o	TStatistics: Computes tstatistics for the intercept and slope.
o	PValues: Calculates pvalues for the intercept and slope.

![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/2303f712-e118-44c8-a52a-0c22b7023a16)

 
o	Delta Values: Computes delta values for specified months.
o	Uncertainty in Delta: Calculates the uncertainty in delta values.
o	LTS (LongTerm Stability): Computes longterm stability values.
o	Relative STS: Calculates relative stability as a percentage.

 ![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/85ab9b63-6fbb-40e8-9198-b81c2b23cb7e)

o	Analysis Object: Compiles all computed statistics and metrics into an analysis object.
o	Significance Statement: Determines the significance of the value.
o	Result Object: Creates a result object containing all analysis results.
o	Return Statement: Returns the result object as an HTTP 200 OK response.

![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/936c8226-ab58-47a7-a097-61e6d80ea1da)

o	Exception Handling: Catches any exceptions and returns an HTTP 500 Internal Server Error response with




 
o	Exception Handling: Catches any exceptions and returns an HTTP 500 Internal Server Error response with

