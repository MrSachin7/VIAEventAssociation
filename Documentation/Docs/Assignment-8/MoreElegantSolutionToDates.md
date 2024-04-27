In the word file for assignement-8, you (Troels) have mentioned that the date are saved as string in the database
. But this is only because we are using SQLite. In a real database, we would use the date type to store dates. This would make it easier to query the database for dates. 

While scaffolding, the efc generated classes tends to think the date fields as string.
However, while making the configuration, we can change the data type to date.

```csharp
    // Duration (nullable , multi-valued)
        entityBuilder.OwnsOne(entity => entity.Duration,
            builder => {
                builder.Property(vo => vo.StartDateTime)
                    .HasColumnName("StartDateTime")
                    .HasColumnType("DATETIME");

                builder.Property(vo => vo.EndDateTime)
                    .HasColumnName("EndDateTime")
                    .HasColumnType("DATETIME");
            });

 ```

As you can see above , I have specifically mentioned that these are datetime objects.
Well, this doesnt change the fact that the date is stored as string in the database.
But it does help the entity framework to understand that these are datetime objects and not strings. 
This will not only  help in querying the database for dates but also when scaffolding the database, the date fields will be generated as datetime fields.


```csharp
public partial class VeaEvent
{
    public DateTime? StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }
    
    // Rest of the fields

}
```

As can be seen above, the fields are generated as DateTime objects and not strings.
