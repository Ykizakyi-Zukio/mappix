namespace Mappix.Data
{
    public struct Region(int startIndex, int endIndex, int startStroke, int endStroke)
    {
        public int StartIndex = startIndex, EndIndex = endIndex;
        public int StartStroke = startStroke, EndStroke = endStroke;
    }

    public struct RegionRule(string startPoint = "/*", string endPoint = "*/")
    {
        public string StartPoint = startPoint, EndPoint = endPoint;
    }
}
