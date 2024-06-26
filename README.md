# regression-short-term-stability-api
regression-short-term-stability-api made for Bharat Petroleum Corporation

#Linear Regression in Excel 
Regression analysis output: Summary Output
![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/4faefa38-3fb7-4830-86b3-35e52f150114)

This part tells you how well the calculated linear regression equation fits your source data. Regression analysis output: Summary Output

Multiple R. It is the Correlation Coefficient that measures the strength of a linear relationship between two variables. The correlation coefficient can be any value between -1 and 1, and its absolute value indicates the relationship strength. The larger the absolute value, the stronger the relationship:

1 means a strong positive relationship
-1 means a strong negative relationship
0 means no relationship at all
R Square. It is the Coefficient of Determination, which is used as an indicator of the goodness of fit. It shows how many points fall on the regression line. The R2 value is calculated from the total sum of squares, more precisely, it is the sum of the squared deviations of the original data from the mean.

In our example, R2 is 0.91 (rounded to 2 digits), which is fairly good. It means that 91% of our values fit the regression analysis model. In other words, 91% of the dependent variables (y-values) are explained by the independent variables (x-values). Generally, an R Squared of 95% or more is considered a good fit.

Adjusted R Square. It is the R square adjusted for the number of independent variables in the model. You will want to use this value instead of R square for multiple regression analysis.

Standard Error. It is another goodness-of-fit measure that shows the precision of your regression analysis - the smaller the number, the more certain you can be about your regression equation. While R2 represents the percentage of the dependent variables variance that is explained by the model, Standard Error is an absolute measure that shows the average distance that the data points fall from the regression line.

Observations. It is simply the number of observations in your model.
Regression analysis output: ANOVA
![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/55740c59-1dfd-414a-b252-d44131a85641)

The second part of the output is Analysis of Variance (ANOVA): Regression analysis output: ANOVA
Basically, it splits the sum of squares into individual components that give information about the levels of variability within your regression model:

df is the number of degrees of freedom associated with the sources of variance.
SS is the sum of squares. The smaller the Residual SS compared with the Total SS, the better your model fits the data.
MS is the mean square.
F is the F statistic or F-test for the null hypothesis. It is used to test the overall significance of the model.
Significance F is the P-value of F.
The ANOVA part is rarely used for simple linear regression analysis in Excel, but you should have a close look at the last component. The Significance F value gives an idea of how reliable (statistically significant) your results are. If Significance F is less than 0.05 (5%), your model is OK. If it is greater than 0.05, you'd probably better choose another independent variable.

Regression analysis output: coefficients
![image](https://github.com/prajeeta15/regression-short-term-stability-api/assets/96904203/22e2c872-f2b1-4c39-92ba-31396f4fc31f)

This section provides specific information about the components of your analysis: Regression analysis output: coefficients
The most useful component in this section is Coefficients. It enables you to build a linear regression equation in Excel:
y = bx + a
For the example data set, where y is the number of umbrellas sold and x is the average monthly rainfall, our linear regression formula goes as follows:
Y = Rainfall Coefficient * x + Intercept

Equipped with a and b values rounded to three decimal places, it turns into:

Y=0.45*x-19.074

For example, with the average monthly rainfall equal to 82 mm, the umbrella sales would be approximately 17.8:

0.45*82-19.074=17.8

Similarly, you can find out how many umbrellas are going to be sold with any other monthly rainfall (x variable) you specify.
Regression analysis output: residuals
If you compare the estimated and actual number of sold umbrellas corresponding to the monthly rainfall of 82 mm, you will see that these numbers are slightly different:

Estimated: 17.8 (calculated above)
Actual: 15 (row 2 of the source data)
Why's the difference? Because independent variables are never perfect predictors of the dependent variables. And the residuals can help you understand how far away the actual values are from the predicted values: Regression analysis output: residuals


## Regression Analysis API Documentation

# Project Aim
This project aims to develop an API using ASP.NET Core that performs regression analysis on a given dataset. The API reads a JSON file containing data points, conducts linear regression analysis, and returns various statistical metrics and insights. This project utilizes MathNet.Numerics for numerical computations and statistical analysis.

### Code Explanation

#### Using Statements
```csharp
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
```
- **MathNet.Numerics**: A library for advanced numerical computations.
- **Microsoft.AspNetCore.Mvc**: ASP.NET Core MVC framework for creating web APIs.
- **Newtonsoft.Json**: A library for JSON serialization and deserialization.
- **System.IO and System.Linq**: Standard libraries for input/output operations and LINQ queries.

#### Controller Definition
```csharp
[Route("api/[controller]")]
[ApiController]
public class RegressionController : ControllerBase
{
```
- **Route and ApiController Attributes**: Specifies the route for the API and identifies the class as an API controller.

#### Analyze Method
```csharp
[HttpPost("regression analysis")]
public IActionResult Analyze(IFormFile jsonfile)
```
- **HttpPost Attribute**: Indicates that this method handles POST requests.
- **Analyze Method**: Handles file upload, performs regression analysis, and returns results.

#### File Reading
```csharp
if (jsonfile == null || jsonfile.Length == 0)
{
    return BadRequest("No file uploaded");
}

using var reader = new StreamReader(jsonfile.OpenReadStream());
string json = reader.ReadToEnd();
```
- **File Check**: Validates if the uploaded file is null or empty.
- **File Reading**: Reads the contents of the uploaded JSON file.

#### Data Deserialization
```csharp
var regjson = JsonConvert.DeserializeObject<List<RegressionData>>(json);

if (regjson == null || !regjson.Any())
{
    return BadRequest("Invalid or empty data in the uploaded file");
}
```
- **Data Deserialization**: Converts JSON data into a list of `RegressionData` objects.
- **Data Validation**: Checks if the deserialized data is null or empty.

#### Linear Regression
```csharp
var periods = regjson.Select(d => (double)d.Week).ToArray();
var densities = regjson.Select(d => d.Density).ToArray();
var regression = SimpleRegression.Fit(periods, densities);
var predictions = periods.Select(x => regression.Item1 + regression.Item2 * x).ToArray();
var rSquare = GoodnessOfFit.RSquared(predictions, densities);
```
- **Extracting Data**: Retrieves periods (weeks) and densities from the deserialized data.
- **Performing Regression**: Fits a linear regression model using `SimpleRegression.Fit`.
- **Predictions**: Generates predictions using the regression model.
- **R-Squared Calculation**: Computes the coefficient of determination (R²) to evaluate the model.

#### Statistical Calculations
```csharp
var avgDensity = densities.Average();
var observations = densities.Length;

if (observations < 4)
{
    return BadRequest("Insufficient data points for regression analysis (minimum 3 required).");
}

var mean = densities.Average();
var sumOfSquares = densities.Sum(d => Math.Pow(d - mean, 2));
var sumOfResiduals = densities.Zip(predictions, (d, p) => d - p).Sum(r => r * r);
var standardError = Math.Sqrt(sumOfResiduals / (observations - 2));
var adjustedRSquare = 1 - ((1 - rSquare) * (observations - 1) / (observations - 2));
var multipleR = Math.Sqrt(rSquare);
```
- **Average Density**: Computes the average density.
- **Observations Check**: Ensures there are at least 3 data points for analysis.
- **Mean Calculation**: Calculates the mean of the densities.
- **Sum of Squares**: Computes the total sum of squares.
- **Sum of Residuals**: Computes the sum of squared residuals.
- **Standard Error**: Calculates the standard error of the regression.
- **Adjusted R-Squared**: Adjusts the R² value for the number of predictors.
- **Multiple R**: Computes the correlation coefficient.

#### ANOVA (Analysis of Variance)
```csharp
var ssTotal = sumOfSquares;
var ssRegression = predictions.Sum(p => Math.Pow(p - mean, 2));
var ssResidual = sumOfResiduals;
var dfTotal = observations - 1;
var dfRegression = 1;
var dfResidual = observations - 2;

var msRegression = ssRegression / dfRegression;
var msResidual = ssResidual / dfResidual;
var fValue = msRegression / msResidual;
var significanceF = 1 - MathNet.Numerics.Distributions.FisherSnedecor.CDF(dfRegression, dfResidual, fValue);
```
- **Sum of Squares**: Computes total, regression, and residual sums of squares.
- **Degrees of Freedom**: Calculates degrees of freedom for total, regression, and residual.
- **Mean Squares**: Computes mean squares for regression and residual.
- **F-Value**: Calculates the F-statistic.
- **Significance F**: Computes the p-value for the F-statistic.

#### Coefficients and Statistics
```csharp
var intercept = regression.Item1;
var slope = regression.Item2;

var seIntercept = Math.Sqrt(standardError * standardError * periods.Sum(x => x * x) / (observations * periods.Sum(x => x * x) - Math.Pow(periods.Sum(), 2)));
var seSlope = Math.Sqrt(standardError * standardError * observations / (observations * periods.Sum(x => x * x) - Math.Pow(periods.Sum(), 2)));

var tStatIntercept = intercept / seIntercept;
var tStatSlope = slope / seSlope;
var pValueIntercept = 2 * (1 - MathNet.Numerics.Distributions.StudentT.CDF(0, 1, Math.Abs(tStatIntercept), dfResidual));
var pValueSlope = 2 * (1 - MathNet.Numerics.Distributions.StudentT.CDF(0, 1, dfResidual, Math.Abs(tStatSlope)));
```
- **Intercept and Slope**: Retrieves the regression coefficients.
- **Standard Errors**: Calculates the standard errors of the intercept and slope.
- **T-Statistics**: Computes t-statistics for the intercept and slope.
- **P-Values**: Calculates p-values for the intercept and slope.

#### Relative STS%
```csharp
int[] months = { 1, 2, 6, 12, 24 };
double[] deltaValues = new double[months.Length];
double[] U_deltaValues = new double[months.Length];
double[] U_LTS = new double[months.Length];

for (int i = 0; i < months.Length; i++)
{
    double delta = slope * months[i];
    deltaValues[i] = delta;

    double deltaU = seSlope * months[i];
    U_deltaValues[i] = deltaU;

    double U_LTS_value = Math.Sqrt(delta * delta + deltaU * deltaU);
    U_LTS[i] = U_LTS_value;
}

var ULTS24 = U_LTS[4];
var RelativeSTS = Math.Round((ULTS24 * 100) / avgDensity, 2);
```
- **Delta Values**: Computes delta values for specified months.
- **Uncertainty in Delta**: Calculates the uncertainty in delta values.
- **LTS (Long-Term Stability)**: Computes long-term stability values.
- **Relative STS%**: Calculates relative stability as a percentage.

#### Result Compilation
```csharp
var analysis = new
{
    Intercept = intercept,
    Slope = slope,
    StandardErrorIntercept = seIntercept,
    StandardErrorSlope = seSlope,
    TStatIntercept = tStatIntercept,
    TStatSlope = tStatSlope,
    PValueIntercept = pValueIntercept,
    PValueSlope = pValueSlope,
    Lower95Intercept = intercept - (confidenceInterval * seIntercept),
    Upper95Intercept = intercept + (confidenceInterval * seIntercept),
    Lower95Slope = slope - (confidenceInterval * seSlope),
    Upper95Slope = slope + (confidenceInterval * seSlope)
};

var pValueSignificance = pValueSlope < 0.05 ? "Since P-value is less than 0.05 there is a significant trend." : "Since P-value is greater than 0.05 there is no significant trend.";

var result = new
{
    RegressionStatistics = regressionStatistics,
    AnovaStatistics = anovaStatistics,
    Analysis = analysis,
    PValueSignificance = pValueSignificance,
    DeltaValues = deltaValues,
    U_DeltaValues = U_deltaValues,
    ULTS = U_LTS,
    Relative_STS = RelativeSTS
};

return Ok(result);
```
- **Analysis Object**: Compiles all computed statistics and metrics into an analysis object.
- **Significance Statement**: Determines the significance of the p-value.
- **Result Object**: Creates a result object containing all analysis results.
- **Return Statement**: Returns the result object as an HTTP 200 OK response.

#### Exception Handling
```csharp
catch (Exception ex)
{
    return StatusCode(500, $"Internal server error: {ex.Message}");
}
```
- **Exception Handling**: Catches any exceptions and returns an HTTP 500 Internal Server Error response with

