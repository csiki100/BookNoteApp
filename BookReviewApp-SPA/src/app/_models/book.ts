import { Chapter } from './chapter';
import { Picture } from './picture';


export interface Book{
    id: number;
    name: string;
    chapters: Chapter[];
    picture: Picture;


}