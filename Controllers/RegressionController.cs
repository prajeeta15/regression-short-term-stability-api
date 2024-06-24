using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class RegressionController : ControllerBase
{
    [HttpPost("regression analysis")]
    public IActionResult Analyze(IFormFile jsonfile)
    {
        try
        {
            if (jsonfile == null || jsonfile.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            // Read the uploaded file
            using var reader = new StreamReader(jsonfile.OpenReadStream());
            string json = reader.ReadToEnd();

            // Deserialize JSON data
            var regjson = JsonConvert.DeserializeObject<List<RegressionData>>(json);

            if (regjson == null || !regjson.Any())
            {
                return BadRequest("Invalid or empty data in the uploaded file");
            }

            // Perform Linear Regression
            var periods = regjson.Select(d => (double)d.Week).ToArray();
            var densities = regjson.Select(d => d.Density).ToArray();
            var regression = SimpleRegression.Fit(periods, densities);
            var predictions = periods.Select(x => regression.Item1 + regression.Item2 * x).ToArray();
            var rSquare = GoodnessOfFit.RSquared(predictions, densities);

            var avgDensity = densities.Average();

            //check the valid observations
            var observations = densities.Length;
            if (observations < 4)
            {
                return BadRequest("Insufficient data points for regression analysis (minimum 3 required).");
            }

            // Calculate other statistics
            var mean = densities.Average();
            var sumOfSquares = densities.Sum(d => Math.Pow(d - mean, 2));
            var sumOfResiduals = densities.Zip(predictions, (d, p) => d - p).Sum(r => r * r);
            var standardError = Math.Sqrt(sumOfResiduals / (observations - 2));
            var adjustedRSquare = 1 - ((1 - rSquare) * (observations - 1) / (observations - 2));
            var multipleR = Math.Sqrt(rSquare);

            var regressionStatistics = new
            {
                MultipleR = multipleR,
                RSquare = rSquare,
                AdjustedRSquare = adjustedRSquare,
                StandardError = standardError,
                Observations = observations
            };

            // Perform ANOVA
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

            var anovaStatistics = new
            {
                DfRegression = dfRegression,
                DfResidual = dfResidual,
                DfTotal = dfTotal,
                SsRegression = ssRegression,
                SsResidual = ssResidual,
                MsRegression = msRegression,
                MsResidual = msResidual,
                FValue = fValue,
                SignificanceF = significanceF
            };

            // Calculate coefficients and other statistics
            var intercept = regression.Item1;
            var slope = regression.Item2;
            // Calculate standard error of intercept and slope
            var seIntercept = Math.Sqrt(standardError * standardError * periods.Sum(x => x * x) / (observations * periods.Sum(x => x * x) - Math.Pow(periods.Sum(), 2)));
            var seSlope = Math.Sqrt(standardError * standardError * observations / (observations * periods.Sum(x => x * x) - Math.Pow(periods.Sum(), 2)));

            var tStatIntercept = intercept / seIntercept;
            var tStatSlope = slope / seSlope;
            var pValueIntercept = 2 * (1 - MathNet.Numerics.Distributions.StudentT.CDF(0,1, Math.Abs(tStatIntercept), dfResidual));
            var pValueSlope = 2 * (1 - MathNet.Numerics.Distributions.StudentT.CDF(0, 1, dfResidual, Math.Abs(tStatSlope)));
            var confidenceInterval = 1.96; // 95% confidence interval

            //calculating Relative STS%
            int[] months = { 1, 2, 6, 12, 24 };
            double[] deltaValues = new double[months.Length];
            double[] U_deltaValues = new double[months.Length];
            double[] U_LTS = new double[months.Length];
            

            for (int i=0; i<months.Length;i++)
            {
                double delta = slope * months[i];
                deltaValues[i] = delta;

                double deltaU = seSlope * months[i];
                U_deltaValues[i] = deltaU;

                double U_LTS_value = Math.Sqrt(delta * delta + deltaU * deltaU);
                U_LTS[i] = U_LTS_value;

            }

            var ULTS24 = U_LTS[4];
            var RelativeSTS = Math.Round((ULTS24 * 100) / avgDensity,2);

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
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

public class RegressionData
{
    public int Week { get; set; }
    public double Density { get; set; }
}
