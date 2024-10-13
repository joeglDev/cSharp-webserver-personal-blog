namespace Db;
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
}