import { Chapter } from './chapter';
import { Picture } from './picture';

/**
 * @description Interface that represents a Book Entity
 */
export interface Book {
  /**
   * @param id the Id of the Book
   */
  id: number;

  /**
   * @param name the Name of the Book
   */
  name: string;

  /**
   * @param chapters the Chapters of the Book
   */
  chapters: Chapter[];

  /**
   * @param picture the Picture of the Book
   */
  picture: Picture;
}