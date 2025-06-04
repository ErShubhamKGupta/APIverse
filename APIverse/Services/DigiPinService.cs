namespace APIverse.Services
{
    public class DigiPinService
    {
        private readonly char[,] Grid = new char[4, 4]
        {
            { 'F', 'C', '9', '8' },
            { 'J', '3', '2', '7' },
            { 'K', '4', '5', '6' },
            { 'L', 'M', 'P', 'T' }
        };

        public string Generate(double lat, double lon)
        {
            double minLat = 2.5, maxLat = 38.5;
            double minLon = 63.5, maxLon = 99.5;

            if (lat < minLat || lat > maxLat)
                return "Latitude Out of Range";

            if (lon < minLon || lon > maxLon)
                return "Longitude Out of Range";

            string digiPin = "";

            for (int level = 1; level <= 10; level++)
            {
                double latDivDeg = (maxLat - minLat) / 4.0;
                double lonDivDeg = (maxLon - minLon) / 4.0;

                int row = 0, column = 0;

                for (int x = 0; x < 4; x++)
                {
                    double upperLat = maxLat - (x * latDivDeg);
                    double lowerLat = upperLat - latDivDeg;

                    if (lat >= lowerLat && lat < upperLat)
                    {
                        row = x;
                        maxLat = upperLat;
                        minLat = lowerLat;
                        break;
                    }
                }

                for (int x = 0; x < 4; x++)
                {
                    double lowerLon = minLon + (x * lonDivDeg);
                    double upperLon = lowerLon + lonDivDeg;

                    if ((lon >= lowerLon && lon < upperLon) || x == 3)
                    {
                        column = x;
                        minLon = lowerLon;
                        maxLon = upperLon;
                        break;
                    }
                }

                digiPin += Grid[row, column];

                if (level == 3 || level == 6)
                {
                    digiPin += "-";
                }
            }

            return digiPin;
        }
    }
}
