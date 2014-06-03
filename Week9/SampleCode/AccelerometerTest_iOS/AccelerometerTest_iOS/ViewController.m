//
//  ViewController.m
//  AccelerometerTest_iOS
//
//  Created by Elliot Saba on 6/2/14.
//  Copyright (c) 2014 University of Washington. All rights reserved.
//

#import "ViewController.h"

@interface ViewController ()

@end

@implementation ViewController

- (void)viewDidLoad
{
    [super viewDidLoad];
	// Do any additional setup after loading the view, typically from a nib.
    
    
    // Allocate and initialize CMMotionManager instance
    // This syntax is equivalent to the following in C++:
    //
    // this->motionManager = CMMotionManager::alloc();
    // this->motionManager->init();
    //
    motionManager = [[CMMotionManager alloc] init];
    
    // set an update interval
    motionManager.accelerometerUpdateInterval = .2;
    
    // Tell the motionMAnager to start performing accelerometer updates, and call this
    [motionManager startAccelerometerUpdatesToQueue:[NSOperationQueue currentQueue]
                                        withHandler:^(CMAccelerometerData *accelerometerData, NSError *error) {
        NSLog(@"Got acceleration data!");
        [progress setProgress: accelerometerData.acceleration.x];
    }];
    
    // Set our progress bar to a default 0.5f
    [progress setProgress: 0.5f];
    
    
    
    // Because the simulator does not support the CoreMotion coolness, we will
    // generate our own events on a timer, and show those instead
    if( false ) {
        // Start a timer that simulates data coming in
        [NSTimer scheduledTimerWithTimeInterval:0.1
                                         target:self
                                       selector:@selector(createFalseAccelerometerData:)
                                       userInfo:nil
                                        repeats:YES];
    }
}

- (void)createFalseAccelerometerData:(NSTimer *)timer
{
    float randomValue = (float)rand()/RAND_MAX;
    [progress setProgress: randomValue];
}

- (IBAction) buttonPressed
{
    [label setText: @"Hello, EE 590!"];
}






// Non-interesting stuff
- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


@end
