namespace v2.Db;
public class DatabaseCommands
{
    public string CreateBlogPostTable = @"CREATE TABLE IF NOT EXISTS blogposts (
            Id SERIAL PRIMARY KEY,
            Author VARCHAR(255) DEFAULT 'unassigned author',
            Title VARCHAR(255) DEFAULT 'unassigned title',
            Content TEXT,
            TimeStamp TIMESTAMP WITHOUT TIME ZONE,
            Likes INTEGER DEFAULT 0
        );";

    public string CreateImageTable = @"CREATE TABLE IF NOT EXISTS images (
    id SERIAL PRIMARY KEY,
    blogpost_id INTEGER NOT NULL,
    name TEXT,
    img BYTEA,
    
    CONSTRAINT fk_blogpost_id FOREIGN KEY (blogpost_id)
        REFERENCES blogposts(Id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
)";

    public string SelectAllBlogPosts = "SELECT * FROM blogposts;";

    public string InsertBlogPost = @"
            INSERT INTO BlogPosts (
                Author,
                Title,
                Content,
                TimeStamp,
                Likes
            )
            VALUES (
                :Author,
                :Title,
                :Content,
                :TimeStamp,
                :Likes
            )
            RETURNING Id";

    public string DeleteBlogPost = @"DELETE FROM blogposts WHERE Id = :Id";

    public string UpdateBlogPost = @"UPDATE BlogPosts SET Author=:Author, Title=:Title, Content=:Content, TimeStamp=TimeStamp, Likes=:Likes WHERE Id = :Id RETURNING *;";

    public string InsertIntoBlogPostsIfEmpty = @"INSERT INTO blogposts (Author, Title, Content, TimeStamp, Likes)
SELECT :author, :title, :content, :timestamp, :likes
WHERE NOT EXISTS (
    SELECT 1 FROM blogposts WHERE Author = :author AND Title = :title
);";

    public string InsertIntoImageTableIfEmpty = @"
INSERT INTO images (blogpost_id, name, img)
SELECT :blogpostId, :name, :img
WHERE NOT EXISTS (
    SELECT 1 FROM images WHERE blogpost_id = :blogpostId
)";

    public string SelectAllImages = "SELECT * FROM images;";

    public string SelectImage = "SELECT * FROM images WHERE blogpost_id = :blogpost_id;"; // TODO replicate this below

    public string InsertImage = @"
INSERT INTO images (blogpost_id, name, img)
VALUES (:blogpostId, :name, :img) RETURNING ID";

    public string DeleteImage = "DELETE FROM images WHERE blogpost_id = :blogpost_id";
}