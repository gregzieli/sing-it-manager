import mongoose from "mongoose";

const SongSchema = new mongoose.Schema(
    {
        title: {
            type: String,
            required: true,
        },
        artist: {
            type: String,
            required: true,
        },
        featured: String,
    },
    { timestamps: true }
);

export default SongSchema;
