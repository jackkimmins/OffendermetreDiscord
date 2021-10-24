using Microsoft.ML;
using Microsoft.ML.Data;

namespace OffendermetreDiscord
{
    //Used for returning a prediction on a given input.
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }

    //Used for classifying an input.
    public class SentimentIssue
    {
        [LoadColumn(0)]
        public bool Label { get; set; }
        [LoadColumn(2)]
        public string Text { get; set; }
    }

    //Responsible for handling all Offendermetre ML operations.
    class OffendermetreML
    {
        private PredictionEngine<SentimentIssue, SentimentPrediction> predictionEngine;
        private bool isModelLoaded = false;
        private const string MODAL_PATH = "OffendermetreModel.zip";

        //Loads the pre-trained model from file and creates a new prediction engine.
        private void loadModel()
        {
            consoleText.WriteLine($"Loading 'Offendermetre' Model ({MODAL_PATH})...");

            MLContext mlContext = new MLContext(seed: 1);
            DataViewSchema predictionPipelineSchema;
            ITransformer trainedModel = mlContext.Model.Load(MODAL_PATH, out predictionPipelineSchema);
            predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentIssue, SentimentPrediction>(trainedModel);

            consoleText.WriteLine("Loaded 'Offendermetre' Model.\n");
            isModelLoaded = true;
        }

        //To achieve a higher accuracy, the model is only trained on characters in the following regex pattern.
        //This function formats inputs to be predicted to the expected pattern.
        private void CleanText(ref string msg)
        {
            msg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 -]").Replace(msg, "");
        }

        //Determines whether a given input is offensive and returns our sentiment prediction.
        public SentimentPrediction Query(string msg)
        {
            //Checks if the model is already loaded.
            if (!isModelLoaded)
                loadModel();

            SentimentIssue sampleStatement = new SentimentIssue { Text = msg };
            return predictionEngine.Predict(sampleStatement);
        }
    }
}
