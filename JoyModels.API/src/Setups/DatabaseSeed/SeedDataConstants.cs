using JoyModels.Models.Enums;

namespace JoyModels.API.Setups.DatabaseSeed;

public static class SeedDataConstants
{
    public static readonly Random Random = new();

    public static readonly string[] AvatarColors =
    [
        "#3498db", "#e74c3c", "#2ecc71", "#9b59b6", "#f39c12", "#1abc9c"
    ];

    public static readonly
        List<(Guid Uuid, string Email, string Nickname, string FirstName, string LastName, string Role)> FixedUsers =
        [
            (Guid.Parse("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), "root1@joymodels.com", "root1", "Root", "One",
                nameof(UserRoleEnum.Root)),
            (Guid.Parse("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"), "root2@joymodels.com", "root2", "Root", "Two",
                nameof(UserRoleEnum.Root)),
            (Guid.Parse("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f"), "admin1@joymodels.com", "admin1", "Admin", "One",
                nameof(UserRoleEnum.Admin)),
            (Guid.Parse("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8a"), "admin2@joymodels.com", "admin2", "Admin", "Two",
                nameof(UserRoleEnum.Admin)),
            (Guid.Parse("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b"), "user1@joymodels.com", "user1", "User", "One",
                nameof(UserRoleEnum.User)),
            (Guid.Parse("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c"), "user2@joymodels.com", "user2", "User", "Two",
                nameof(UserRoleEnum.User)),
            (Guid.Parse("a7b8c9d0-e1f2-4a3b-4c5d-6e7f8a9b0c1d"), "user3@joymodels.com", "user3", "User", "Three",
                nameof(UserRoleEnum.User)),
            (Guid.Parse("b8c9d0e1-f2a3-4b4c-5d6e-7f8a9b0c1d2e"), "user4@joymodels.com", "user4", "User", "Four",
                nameof(UserRoleEnum.User))
        ];

    public static readonly string[] ModelColors =
    [
        "#1a1a2e", "#16213e", "#0f3460", "#1b262c", "#2d4059", "#222831",
        "#1e3163", "#0d1b2a", "#1b2838", "#171717", "#212529", "#343a40",
        "#2c3e50", "#34495e", "#1c2833", "#17202a", "#1a237e", "#0d47a1",
        "#01579b", "#006064", "#004d40", "#1b5e20", "#33691e", "#827717"
    ];

    public static readonly string[] FirstNames =
    [
        "Alex", "Jordan", "Taylor", "Morgan", "Casey", "Riley", "Quinn", "Avery", "Peyton", "Cameron",
        "Skyler", "Dakota", "Reese", "Finley", "Sage", "Rowan", "Emery", "Phoenix", "Harley", "Blake",
        "Drew", "Hayden", "Jamie", "Logan", "Parker", "Emma", "Liam", "Olivia", "Noah", "Ava",
        "Ethan", "Sophia", "Mason", "Isabella", "Lucas", "Mia", "Oliver", "Charlotte", "Elijah", "Amelia",
        "James", "Harper", "Benjamin", "Evelyn", "Henry", "Abigail", "Sebastian", "Emily", "Jack", "Elizabeth",
        "Aiden", "Sofia", "Owen", "Ella", "Samuel", "Scarlett", "Ryan", "Grace", "Nathan", "Chloe",
        "Caleb", "Victoria", "Christian", "Aria", "Dylan", "Lily", "Isaac", "Zoey", "Joshua", "Penelope",
        "Andrew", "Layla", "Daniel", "Nora", "Matthew", "Riley", "Joseph", "Zoe", "David", "Hannah",
        "Carter", "Hazel", "Luke", "Luna", "Gabriel", "Savannah", "Anthony", "Audrey", "Lincoln", "Brooklyn",
        "Jaxon", "Bella", "Asher", "Claire", "Christopher", "Skylar", "Ezra", "Lucy", "Theodore", "Paisley"
    ];

    public static readonly string[] LastNames =
    [
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
        "Wilson", "Anderson", "Taylor", "Thomas", "Moore", "Jackson", "Martin", "Lee", "Thompson", "White",
        "Harris", "Clark", "Lewis", "Robinson", "Walker", "Hall", "Allen", "Young", "King", "Wright",
        "Scott", "Green", "Baker", "Adams", "Nelson", "Carter", "Mitchell", "Perez", "Roberts", "Turner",
        "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez", "Morris", "Rogers",
        "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey", "Rivera", "Cooper", "Richardson", "Cox",
        "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez", "James", "Watson", "Brooks", "Kelly",
        "Sanders", "Price", "Bennett", "Wood", "Barnes", "Ross", "Henderson", "Coleman", "Jenkins", "Perry",
        "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington", "Butler", "Simmons", "Foster", "Gonzales",
        "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Hayes", "Myers", "Ford", "Hamilton", "Graham"
    ];

    public static readonly string[] ModelNamePrefixes =
    [
        "Ancient", "Modern", "Futuristic", "Vintage", "Classic", "Epic", "Mystic",
        "Crystal", "Shadow", "Golden", "Silver", "Dark", "Light", "Royal", "Noble"
    ];

    public static readonly string[] ModelNameNouns =
    [
        "Dragon", "Knight", "Castle", "Sword", "Shield", "Tower", "Temple",
        "Warrior", "Mage", "Throne", "Crown", "Armor", "Helmet", "Fortress",
        "Phoenix", "Griffin", "Unicorn", "Golem", "Titan", "Sphinx"
    ];

    public static readonly string[] ModelDescriptions =
    [
        "A highly detailed 3D model perfect for game development and visualization projects.",
        "Meticulously crafted with attention to every polygon and texture detail.",
        "Optimized for real-time rendering while maintaining visual fidelity.",
        "Created using industry-standard techniques for maximum compatibility.",
        "Features clean topology suitable for animation and rigging.",
        "Includes multiple LOD versions for performance optimization.",
        "Hand-sculpted details combined with procedural texturing.",
        "Perfect for architectural visualization and product rendering."
    ];

    public static readonly string[] FaqQuestions =
    [
        "What software was used to create this model?",
        "Is this model rigged for animation?",
        "What file formats are included in the download?",
        "Can I use this model for commercial projects?",
        "What is the polygon count of this model?",
        "Are textures included with this model?",
        "Is this model suitable for 3D printing?",
        "Can you provide a lower poly version?",
        "What rendering engine was used for the preview images?",
        "Does this model include UV mapping?",
        "Are there any known issues with the model?",
        "Can I request modifications to this model?",
        "What scale is this model created in?",
        "Is the model optimized for game engines?",
        "Do you offer support after purchase?"
    ];

    public static readonly string[] FaqAnswers =
    [
        "This model was created using Blender and ZBrush for high-detail sculpting.",
        "Yes, the model is fully rigged and ready for animation in most 3D software.",
        "The download includes OBJ, FBX, and GLTF formats for maximum compatibility.",
        "Yes, this model comes with a commercial license for use in any project.",
        "The model has approximately 50,000 polygons in the high-poly version.",
        "Yes, all textures are included in 4K resolution (diffuse, normal, roughness).",
        "The model can be 3D printed but may require some cleanup for best results.",
        "I can provide a lower poly version upon request, just send me a message.",
        "The preview images were rendered in Cycles with HDRI lighting.",
        "Yes, the model includes properly unwrapped UV maps for easy texturing.",
        "No known issues. If you find any problems, please let me know!",
        "Yes, I offer custom modifications for an additional fee. Contact me for details.",
        "The model is created at real-world scale (1 unit = 1 meter).",
        "Yes, it's optimized for Unity and Unreal Engine with LOD support.",
        "Absolutely! I provide support for any issues you might encounter.",
        "Thank you for your interest! Feel free to reach out with any questions.",
        "Great question! The answer depends on your specific use case.",
        "I appreciate your feedback and will consider this for future updates.",
        "Please check the product description for more detailed specifications.",
        "I'm happy to help! Let me know if you need anything else."
    ];

    public static readonly string[] PositiveReviewTexts =
    [
        "Excellent quality model! The details are amazing and it works perfectly in my project.",
        "Very impressed with this model. Clean topology and great textures included.",
        "Best purchase I've made. The model is exactly as described and easy to use.",
        "Fantastic work! The artist clearly put a lot of effort into this piece.",
        "Highly recommend this model. Perfect for game development projects.",
        "Outstanding quality for the price. Will definitely buy from this creator again.",
        "The model exceeded my expectations. Great attention to detail throughout.",
        "Perfect for my needs. The file formats provided made integration seamless.",
        "Amazing model with professional quality. The creator was also very helpful.",
        "Five stars! This model saved me hours of work on my project."
    ];

    public static readonly string[] NegativeReviewTexts =
    [
        "The model quality doesn't match the preview images. Disappointed with this purchase.",
        "Had some issues with the topology. Not ideal for animation purposes.",
        "The textures were lower resolution than expected. Needs improvement.",
        "Model required significant cleanup before I could use it in my project.",
        "Not worth the price. I've seen better models for less money.",
        "The file formats provided were outdated. Had compatibility issues.",
        "Description was misleading. The model lacks many advertised features.",
        "Polygon count was much higher than stated, causing performance issues.",
        "UV mapping needs work. Had to redo most of it myself.",
        "Customer support was slow to respond when I had questions."
    ];

    public static readonly string[] CommunityPostColors =
    [
        "#6B2D5C", "#4A1942", "#2D132C", "#801336", "#C72C41",
        "#0B3D91", "#1E5128", "#4E3524", "#2C3E50", "#1A1A2E",
        "#FF6B35", "#004E89", "#7B2D26", "#3D5A80", "#293241"
    ];

    public static readonly string[] CommunityPostTitles =
    [
        "My first game dev project - feedback welcome!",
        "Tutorial: Creating realistic water shaders",
        "Showcase: Low-poly character I made this week",
        "Tips for optimizing Unity projects",
        "How I improved my Blender workflow",
        "Question about PBR texturing",
        "My indie game progress update",
        "Best practices for game UI design",
        "Procedural generation techniques I learned",
        "Retro pixel art style guide",
        "Animation tips for beginners",
        "Level design principles that work",
        "My journey learning Unreal Engine",
        "Shader Graph tutorial for beginners",
        "Game jam experience and lessons learned",
        "How to create stylized environments",
        "VFX breakdown of my latest effect",
        "Character rigging workflow tips",
        "Sound design basics for games",
        "Post-processing effects guide",
        "Mobile game optimization tricks",
        "Creating immersive game worlds",
        "Substance Painter workflow tips",
        "Making modular game assets",
        "AI in games: My experiments"
    ];

    public static readonly string[] CommunityPostDescriptions =
    [
        "I've been working on this project for the past few months and would love to get some feedback from the community. Any suggestions for improvement are welcome!",
        "In this guide, I'll walk you through the process step by step. I've learned a lot from this community and wanted to give back by sharing what I know.",
        "Here's something I've been working on lately. It took me about a week to complete and I'm pretty happy with how it turned out. Let me know what you think!",
        "After struggling with this for a while, I finally figured out a workflow that works for me. Sharing it here in case it helps someone else.",
        "I've compiled some tips and tricks that have significantly improved my productivity. Hope this helps other developers in their journey!",
        "Been experimenting with different techniques and wanted to share my results. The difference in quality is quite noticeable.",
        "This is my progress so far on my indie game project. Still a lot of work to do but I'm excited about the direction it's going.",
        "I've gathered some resources and techniques that have helped me improve significantly. Sharing them with the community!",
        "After watching countless tutorials and practicing for months, here's what I've learned. These tips really made a difference for me.",
        "Wanted to document my learning journey and share it with others who might be starting out. We all start somewhere!",
        "Here's a breakdown of my creative process for this project. I hope it gives some insight into how I approach these challenges.",
        "I've been following this community for a while and finally decided to share my own work. Feedback is greatly appreciated!",
        "This tutorial covers everything from the basics to more advanced techniques. Perfect for those looking to level up their skills.",
        "Sharing my latest creation with the community. It's been a challenging but rewarding experience putting this together.",
        "I've put together a comprehensive guide based on my experience. Hope it helps others avoid the mistakes I made early on."
    ];

    public static readonly string[] CommunityPostComments =
    [
        "This is really impressive work! How long did it take you to learn these techniques?",
        "Great tutorial! I've been looking for something like this for a while.",
        "Love the art style! What software did you use for this?",
        "Thanks for sharing! This will definitely help with my current project.",
        "Awesome work! Any plans to create more tutorials like this?",
        "This is exactly what I needed. Bookmarking for later!",
        "Really well explained. Even a beginner like me could follow along.",
        "The attention to detail here is incredible. Well done!",
        "I tried this technique and it worked great. Thanks for the tip!",
        "Would love to see more content like this from you.",
        "How do you handle the performance impact of this approach?",
        "This inspired me to try something similar. Great work!",
        "Clean and professional looking. What's your workflow?",
        "I've been struggling with this exact problem. Thanks for the solution!",
        "Mind sharing the settings you used for this effect?"
    ];

    public static readonly string[] CommunityPostAnswers =
    [
        "Thanks for the kind words! It took me about 6 months of practice to get to this level.",
        "Glad you found it helpful! I used Blender for the modeling and Substance Painter for textures.",
        "I'm planning to create more tutorials in the future. Stay tuned!",
        "The performance impact is minimal if you follow the optimization tips I mentioned.",
        "My workflow involves planning everything out first, then executing in stages.",
        "Feel free to reach out if you have any questions about the process!",
        "I'll definitely share more settings and configurations in my next post.",
        "Thanks! I spent a lot of time on the small details to make it look right.",
        "Happy to help! Let me know if you need any clarification on the steps.",
        "I appreciate the feedback! It motivates me to create more content."
    ];

    public static readonly string[] YoutubeVideoLinks =
    [
        "https://www.youtube.com/watch?v=i_UEP_Mkiqs",
        "https://www.youtube.com/watch?v=TPrnSACiTJ4",
        "https://www.youtube.com/watch?v=IKqJPuCfvmo",
        "https://www.youtube.com/watch?v=gB1F9G0JXOo",
        "https://www.youtube.com/watch?v=pwZpJzpE2lQ",
        "https://www.youtube.com/watch?v=_nRzoTzeyxU",
        "https://www.youtube.com/watch?v=QMU0qR_GGFw",
        "https://www.youtube.com/watch?v=RqRoXLLwJ8g",
        "https://www.youtube.com/watch?v=aircAruvnKk",
        "https://www.youtube.com/watch?v=Z1qyvQsjK5Y"
    ];

    public static readonly string[] ReportDescriptions =
    [
        "This content violates community guidelines and should be reviewed immediately.",
        "I've seen this user posting similar content multiple times.",
        "This appears to be automated spam behavior.",
        "The content contains misleading information.",
        "This user has been harassing other community members.",
        "The content appears to be stolen from another creator.",
        "Multiple users have complained about this content.",
        "This violates the terms of service.",
        "The behavior is disruptive to the community.",
        "Please review this content as soon as possible."
    ];
}