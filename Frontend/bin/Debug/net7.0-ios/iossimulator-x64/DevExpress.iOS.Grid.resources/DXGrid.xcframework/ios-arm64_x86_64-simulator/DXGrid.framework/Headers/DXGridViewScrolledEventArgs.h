#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface DXGridViewScrolledEventArgs : NSObject

@property (readonly, nonatomic) CGFloat deltaX;
@property (readonly, nonatomic) CGFloat deltaY;
@property (readonly, nonatomic) CGFloat offsetX;
@property (readonly, nonatomic) CGFloat offsetY;
@property (readonly, nonatomic) int firstVisibleRowIndex;
@property (readonly, nonatomic) int lastVisibleRowIndex;
@property (readonly, nonatomic) CGFloat viewportWidth;
@property (readonly, nonatomic) CGFloat viewportHeight;
@property (readonly, nonatomic) CGFloat extentWidth;
@property (readonly, nonatomic) CGFloat extentHeight;

-(void)set:(CGFloat)deltaX andDeltaY:(CGFloat)deltaY
             andOffsetX:(CGFloat)offsetX andOffsetY:(CGFloat)offsetY
             andFirstVisibleRowIndex:(int)firstVisibleRowIndex andLastVisibleRowIndex:(int)lastVisibleRowIndex
             andViewportWidth:(CGFloat)viewportWidth andViewportHeight:(CGFloat)viewportHeight
            andExtentWidth:(CGFloat)extentWidth andExtentHeight:(CGFloat)extentHeight;
@end

NS_ASSUME_NONNULL_END

