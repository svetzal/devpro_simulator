using System;

namespace DevProSimulator
{
    public class StoryDeck
    {
        private int RandomAmount => StorySettings.Sizes[new Random().Next(StorySettings.Sizes.Length)];

        public Story NextFeature()
        {
            var description = Faker.Lorem.Sentence();
            return new Story(RandomAmount, RandomAmount, description, Story.FeatureType);
        }

        public Story CreateDefect(Story story)
        {
            var defect = new Story(0, RandomAmount, story.Description, Story.DefectType);
            story.AddDefect(defect);
            return defect;
        }
    }
}