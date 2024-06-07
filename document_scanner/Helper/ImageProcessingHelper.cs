using System;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;

namespace document_scanner.Helper
{
    public static class ImageProcessingHelper
    {
        public static bool IsDocumentCentered(Mat image, int maximumDeviation)
        {
            var imageCenter = new Point(image.Cols / 2, image.Rows / 2);
            var documentContour = FindDocumentContour(image);

            if (documentContour != null)
            {
                var documentCenter = GetContourCenter(documentContour);

                var distance = Math.Sqrt(Math.Pow(imageCenter.X - documentCenter.X, 2) +
                             Math.Pow(imageCenter.Y - documentCenter.Y, 2));

                var imageDiagonal = Math.Sqrt(Math.Pow(image.Cols, 2) + Math.Pow(image.Rows, 2));
                var deviationThreshold = maximumDeviation * imageDiagonal / 100;

                return distance <= deviationThreshold;
            }

            return false;
        }

        public static bool IsDocumentFullyVisible(Mat image, double minimumVisibilityThreshold)
        {
            var documentContour = FindDocumentContour(image);

            if (documentContour != null)
            {
                var documentArea = Cv2.ContourArea(documentContour);
                var imageArea = image.Cols * image.Rows;

                return documentArea / imageArea >= minimumVisibilityThreshold;
            }

            return false;
        }

        public static bool IsBrightnessCorrect(Mat image, int requiredBrightness)
        {
            var brightness = CalculateBrightness(image);
            return brightness >= requiredBrightness;
        }

        public static double CalculateBrightness(Mat image)
        {
            var grayscaleImage = new Mat();
            Cv2.CvtColor(image, grayscaleImage, ColorConversionCodes.BGR2GRAY);
            return Cv2.Mean(grayscaleImage)[0];
        }

        public static bool IsColorAcceptable(Mat image)
        {
            // TODO
            // Implement logic to check color based on application requirements
            return true;
        }

        private static Point[]? FindDocumentContour(Mat image)
        {
            var grayImage = new Mat();
            Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

            var binaryImage = new Mat();
            Cv2.Threshold(grayImage, binaryImage, 0, 255, ThresholdTypes.Otsu);

            var contours = Cv2.FindContoursAsArray(binaryImage, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length > 0)
            {
                // Filter contours based on size and shape to ensure it's a document
                var documentContours = contours
                    .Where(c => IsContourLikelyDocument(c))
                    .OrderByDescending(c => Cv2.ContourArea(c))
                    .ToArray();

                if (documentContours.Length > 0)
                {
                    return documentContours.First();
                }
            }

            return null;
        }

        private static bool IsContourLikelyDocument(Point[] contour)
        {
            var area = Cv2.ContourArea(contour);
            if (area < 1000) return false; // Arbitrary threshold, adjust based on your needs

            var rect = Cv2.BoundingRect(contour);
            var aspectRatio = Math.Max(rect.Width / (float)rect.Height, rect.Height / (float)rect.Width);

            return aspectRatio < 2.0; // Assuming document is roughly rectangular
        }

        private static Point GetContourCenter(Point[] contour)
        {
            var moments = Cv2.Moments(contour);
            var centroidX = moments.M10 / moments.M00;
            var centroidY = moments.M01 / moments.M00;
            return new Point(centroidX, centroidY);
        }
    }
}
